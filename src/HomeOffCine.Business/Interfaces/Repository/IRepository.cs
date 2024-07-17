using HomeOffCine.Business.Models;
using System.Linq.Expressions;

namespace HomeOffCine.Business.Interfaces.Repository;

public interface IRepository<T> : IDisposable where T : Entity
{
    void Add(T entity);
    void Remove(T entity);
    void Update(T entity);

    Task<T> GetByIdAsync(Guid id);
    Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression);
    Task<int> CountAsync(Expression<Func<T, bool>> expression);

    Task<bool> SaveChanges();
}
