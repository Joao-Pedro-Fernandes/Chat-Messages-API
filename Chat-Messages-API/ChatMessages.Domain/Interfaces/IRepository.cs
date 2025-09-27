using System.Linq.Expressions;

namespace ChatMessages.Domain.Interfaces;

public interface IRepository<T> where T : class
{
    Task<List<T>> GetAllAsync();
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
    Task<T> AddAsync(T entity);
    T Update(T entity);
    T Delete(T entity);
}
