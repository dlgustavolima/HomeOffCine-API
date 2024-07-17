using HomeOffCine.Business.Models;
using HomeOffCine.Infra.Context;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using HomeOffCine.Business.Interfaces.Repository;

namespace HomeOffCine.Infra.Repository;

public abstract class Repository<T> : IRepository<T> where T : Entity, new()
{
    protected readonly DbSet<T> _dbSet;
    protected readonly HomeOffCineDbContext Db;

    public Repository(HomeOffCineDbContext db)
    {
        Db = db;
        _dbSet = db.Set<T>();
    }

    public virtual void Add(T entity)
    {
        _dbSet.Add(entity);
    }

    public virtual void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public virtual void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public virtual async Task<int> CountAsync(Expression<Func<T, bool>> expression)
    {
        return await _dbSet.CountAsync(expression);
    }

    public virtual async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression)
    {
        return await _dbSet.FirstOrDefaultAsync(expression);
    }

    public virtual async Task<T> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<List<T>> GetDataAsync(
        Expression<Func<T, bool>> expression = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
        int? skip = null,
        int? take = null)
    {
        var query = _dbSet.AsQueryable();

        if (expression != null)
        {
            query = query.Where(expression);
        }

        if (include != null)
        {
            query = include(query);
        }

        if (skip != null && skip.HasValue)
        {
            query = query.Skip(skip.Value);
        }

        if (take != null && take.HasValue)
        {
            query = query.Take(take.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<bool> SaveChanges()
    {
        return await Db.SaveChangesAsync() > 0 ? true : false;
    }

    public void Dispose()
    {
        Db?.Dispose();
    }
}
