using ChatMessages.Domain.Interfaces;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ChatMessages.Infrastructure.Context;

namespace ChatMessages.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ChatMessageContext _context;

    public Repository(ChatMessageContext context)
    {
        _context = context;
    }

    public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null)
    {
        if (predicate != null)
            return await _context.Set<T>().AsNoTracking().Where(predicate).ToListAsync();

        return await _context.Set<T>().AsNoTracking().ToListAsync();
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate);
    }
    public async Task<T> AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);

        return entity;
    }

    public T Update(T entity)
    {
        _context.Set<T>().Update(entity);

        return entity;
    }

    public T Delete(T entity)
    {
        _context.Set<T>().Remove(entity);

        return entity;
    }
}
