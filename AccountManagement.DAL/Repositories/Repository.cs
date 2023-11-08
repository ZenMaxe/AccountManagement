using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace AccountManagement.DAL.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext _dbContext;
    protected readonly DbSet<T> _dbSet;

    public Repository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }

    public virtual IEnumerable<T> Get(
        Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        string includeProperties = "")
    {
        // For More Info: https://learn.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application

        IQueryable<T> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }
        foreach (var include in includeProperties.Split(
            new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query.Include(include);
        }
        if (orderBy != null)
        {
            return orderBy(query).ToList();
        }
        else
        {
            return query.ToList();
        }
    }

    public async Task<T?> GetByIdAsync(int id, bool track = true)
    {

        var data =  await _dbSet.FindAsync(id);

        if (!track && data != null)
        {
            _dbContext.Entry(data).State = EntityState.Detached;
        }

        return data;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task AddAsync(T entity)
    {

        await _dbSet.AddAsync(entity);
    }

    public void Update(T _object)
    {
        _dbSet.Update(_object);
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }
    public Task Add(T _object)
    {
        return Task.FromResult(_dbContext.Add(_object));
    }

    public void Delete(T _object)
    {
        _dbSet.Remove(_object);
    }


    public IEnumerable<T> GetAll()
    {
        return _dbSet.ToList();
    }

    public T? GetById(int id)
    {
        return _dbSet.Find(id);
    }

    public IQueryable<T> GetQueryable()
    {
        return _dbSet;
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
    }
    public IQueryable<T> GetManyQueryable(Expression<Func<T, bool>> expression)
    {
        return _dbSet.Where(expression);
    }
    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate);
    }
    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }
    public async Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public void Detach(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _dbContext.Entry(entity).State = EntityState.Detached;
    }

}