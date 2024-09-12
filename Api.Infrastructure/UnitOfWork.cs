using Api.Application.Common;
using Api.Infrastructure.Repositories;
using System.Data;

namespace Api.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection _db;
        private IDbTransaction _transaction = default!;

        public UnitOfWork(DapperContext dapperContext)
        {
            _db = dapperContext.GetOpenConnection() ?? throw new ArgumentNullException(nameof(dapperContext));
        }

        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            if (_transaction == null) _transaction = _db.BeginTransaction();
            return new GenericRepository<T>(_db, _transaction);
        }

        public async Task<int> CompleteAsync()
        {
            await Task.CompletedTask;
            try
            {
                if (_transaction != null)
                {
                    _transaction.Commit();
                }
                return 1; // Success indicator
            }
            catch (Exception ex)
            {
                _transaction?.Rollback();
                // Log the exception or handle it as needed
                throw new Exception("An error occurred while completing the transaction.", ex);
            }
        }

        public IDbTransaction BeginTransaction()
        {
            _transaction = _db.BeginTransaction();
            return _transaction;
        }

        public IDbTransaction BeginReadTransaction()
        {
            // You can choose to set a specific isolation level for read transactions if needed
            _transaction = _db.BeginTransaction(IsolationLevel.ReadCommitted);
            return _transaction;
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _db?.Dispose();
        }
    }
}
