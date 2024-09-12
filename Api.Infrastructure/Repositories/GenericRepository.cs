using Api.Application.Common;
using Api.Domain.Attributes;
using Dapper;
using System.Data;
using System.Reflection;

namespace Api.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IDbConnection _db;
        private readonly IDbTransaction _transaction;

        public GenericRepository(IDbConnection db, IDbTransaction transaction)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _transaction = transaction;
        }

        public async Task<int> AddAsync(T entity)
        {
            var properties = GetColumns();
            // Exclude the primary key from the insert statement
            var insertProperties = properties.Where(p => !IsIdentityKey(p.Key)).ToList();

            var sql = $"INSERT INTO {GetTableName()} ({string.Join(", ", insertProperties.Select(p => p.Key))}) VALUES (@{string.Join(", @", insertProperties.Select(p => p.Value))}); SELECT CAST(SCOPE_IDENTITY() as int);";
            return await _db.ExecuteScalarAsync<int>(sql, entity, _transaction);
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            var sql = $"SELECT {string.Join(", ", GetColumns().Keys)} FROM {GetTableName()} WHERE Id = @Id";
            return await _db.QuerySingleOrDefaultAsync<T>(sql, new { Id = id }, _transaction);
        }

        public async Task<T?> GetByColumnAsync(string columnName, object value)
        {
            var sql = $"SELECT {string.Join(", ", GetColumns().Keys)} FROM {GetTableName()} WHERE {columnName} = @{columnName}";

            // Create a DynamicParameters object
            var parameters = new DynamicParameters();
            parameters.Add($"@{columnName}", value); // Add the parameter dynamically

            return await _db.QuerySingleOrDefaultAsync<T>(sql, parameters, _transaction);
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            var properties = GetColumns();
            var sql = $"UPDATE {GetTableName()} SET {string.Join(", ", properties.Keys.Select(k => $"{k} = @{properties[k]}"))} WHERE Id = @Id";
            var rowsAffected = await _db.ExecuteAsync(sql, entity, _transaction);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = $"DELETE FROM {GetTableName()} WHERE Id = @Id";
            var rowsAffected = await _db.ExecuteAsync(sql, new { Id = id }, _transaction);
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var sql = $"SELECT {string.Join(", ", GetColumns().Keys)} FROM {GetTableName()}";
            return await _db.QueryAsync<T>(sql, transaction: _transaction);
        }

        private Dictionary<string, string> GetColumns()
        {
            var properties = typeof(T).GetProperties();
            var columns = new Dictionary<string, string>();

            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<ColumnNameAttribute>();
                var columnName = attribute?.Name ?? property.Name; // Use property name if attribute is not specified
                columns.Add(columnName, property.Name);
            }

            return columns;
        }

        private bool IsIdentityKey(string propertyName)
        {
            var propertyInfo = typeof(T).GetProperty(propertyName);
            return propertyInfo?.GetCustomAttribute<IdentityColumnAttribute>() != null;
        }

        private string GetTableName()
        {
            var tableAttribute = typeof(T).GetCustomAttribute<TableNameAttribute>();
            return tableAttribute?.Name ?? typeof(T).Name + "s"; // Default to pluralized class name if attribute is not specified
        }
    }
}
