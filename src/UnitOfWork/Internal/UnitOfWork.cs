using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace UnitOfWork.Internal
{
    internal sealed class UnitOfWork(DbContext db) : IUnitOfWork
    {
        public async Task AddAsync<T>(T entity) where T : class
            => await db.Set<T>().AddAsync(entity);

        public async Task AddAsync<T>(T entity, CancellationToken cancellationToken) where T : class
            => await db.Set<T>().AddAsync(entity, cancellationToken);

        public async Task AddRangeAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken) where T : class
            => await db.Set<T>().AddRangeAsync(entities, cancellationToken);

        public async Task CommitAsync(CancellationToken cancellationToken)
            => await db.SaveChangesAsync(cancellationToken);

        public IQueryable<T> Query<T>() where T : class
            => db.Set<T>();

        public void Remove<T>(T entity) where T : class
            => db.Set<T>().Remove(entity);

        public void RemoveRange<T>(IEnumerable<T> entities) where T : class
            => db.Set<T>().RemoveRange(entities);

        public async Task<IDbContextTransaction> BeginSnapshotTransactionAsync(CancellationToken cancellationToken)
        {
            return await db.Database.BeginTransactionAsync(IsolationLevel.Snapshot, cancellationToken);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
        {
            return await db.Database.BeginTransactionAsync(cancellationToken);
        }
    }
}