using EducationPortal.Data.Models;
using EducationPortal.Data.Repo.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EducationPortal.Data.Repo.Repositories;

public class Repository<T, TId> : IRepository<T, TId> where T : class, IBaseEntity<TId>
{
    protected readonly PortalDbContext context;

    public Repository(PortalDbContext context)
    {
        this.context = context;
    }
    
    public IQueryable<T> GetAll()
    {
        return context.Set<T>();
    }

    public IQueryable<T> GetQueryable()
    {
        return context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(TId id)
    {
        return await context.Set<T>().FindAsync(id);
    }

    public async Task<T?> GetSingleOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await context.Set<T>().SingleOrDefaultAsync(predicate);
    }

    public async Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate)
    {
        return await context.Set<T>().Where(predicate).ToListAsync();
    }

    public async Task<bool> InsertAsync(T entity)
    {
        await context.Set<T>().AddAsync(entity);
        return true;
    }
    
    public async Task<bool> UpdateAsync(T entity)
    {
        context.Set<T>().Update(entity);
        return await Task.FromResult(true);
    }
    
    public async Task<bool> DeleteAsync(T entity)
    {
        context.Set<T>().Remove(entity);
        return await Task.FromResult(true);
    }

    public async Task<bool> DeleteAsync(TId id)
    {
        T? entityToDelete = await GetByIdAsync(id);
        if (entityToDelete != null)
        {
            return await DeleteAsync(entityToDelete);
        }
        return false;
    }
}