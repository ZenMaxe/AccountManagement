using AccountManagement.DAL.Repositories;

namespace AccountManagement.DAL.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext dbContext;
    private bool disposed = false;
    private readonly Dictionary<Type, object> repositories;



    public UnitOfWork(AppDbContext db)
    {
        dbContext = db;
        repositories = new Dictionary<Type, object>();
    }
    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
    {
        if (repositories.ContainsKey(typeof(TEntity)))
        {
            return (IRepository<TEntity>)repositories[typeof(TEntity)];
        }

        var repository = new Repository<TEntity>(dbContext);
        repositories.Add(typeof(TEntity), repository);
        return repository;
    }
    public int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        return dbContext.SaveChanges(acceptAllChangesOnSuccess);
    }

    public int SaveChanges()
    {
        return dbContext.SaveChanges();
    }
    public async Task BeginTransactionAsync()
    {
        await dbContext.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await dbContext.Database.CurrentTransaction.CommitAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await dbContext.Database.CurrentTransaction.RollbackAsync();
    }
    public async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }
    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                dbContext.Dispose();
            }
        }
        this.disposed = true;
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}