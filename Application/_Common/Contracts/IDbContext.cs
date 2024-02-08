using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Application._Common.Contracts
{
    public interface IDbContext
    {
        DbSet<T> Set<T>() where T : class;

        EntityEntry<TEntity> Entry<TEntity>([NotNull] TEntity entity)
            where TEntity : class;
        EntityEntry<TEntity> Attach<TEntity>(TEntity entity)
            where TEntity : class;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new());
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new());

        int SaveChanges();
        int SaveChanges(bool acceptAllChangesOnSuccess);

        EntityEntry<TEntity> Remove<TEntity>([NotNull] TEntity entity)
            where TEntity : class;
        EntityEntry Remove([NotNull] object entity);
    }
}
