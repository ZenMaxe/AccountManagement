using AccountManagement.DAL.Repositories;

namespace AccountManagement.DAL.UnitOfWork;

public interface IUnitOfWork : IDisposable
{

    IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    int SaveChanges(bool acceptAllChangesOnSuccess);
    int SaveChanges();
    Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new());
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new());
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    Task BeginTransactionAsync();

}