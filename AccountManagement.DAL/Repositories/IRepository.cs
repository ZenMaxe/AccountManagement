using System.Linq.Expressions;

namespace AccountManagement.DAL.Repositories;

public interface IRepository<T>
{
    public IQueryable<T> GetQueryable();

    public IEnumerable<T> Get(Expression<Func<T, bool>> filter = null,
                              Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                              string includeProperties = "");

    public Task Add(T _object);

    public void Delete(T _object);

    public void Update(T _object);

    public IEnumerable<T> GetAll();
    public Task<IEnumerable<T>> GetAllAsync();

    public T? GetById(int id);
    public Task<T?> GetByIdAsync(int id, bool track = true);
    Task AddAsync(T entity);
    void RemoveRange(IEnumerable<T> entities);
    IQueryable<T> GetManyQueryable(Expression<Func<T, bool>> expression);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate);
    void Detach(T entity);
}