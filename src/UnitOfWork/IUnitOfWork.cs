using System.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace UnitOfWork;

public interface IUnitOfWork
{
    IQueryable<T> Query<T>() where T : class;
    Task AddAsync<T>(T entity, CancellationToken cancellationToken) where T : class;
    Task AddRangeAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken) where T : class;
    void Remove<T>(T entity) where T : class;
    void RemoveRange<T>(IEnumerable<T> entities) where T : class;
    Task CommitAsync(CancellationToken cancellationToken);
    Task<IDbContextTransaction> BeginSnapshotTransactionAsync(CancellationToken cancellationToken);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
    Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken);
}