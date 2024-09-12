using System.Data;

namespace Api.Application.Common
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> GetRepository<T>() where T : class;
        Task<int> CompleteAsync();
        IDbTransaction BeginTransaction(); // For write operations
        IDbTransaction BeginReadTransaction(); // For read operations
    }
}
