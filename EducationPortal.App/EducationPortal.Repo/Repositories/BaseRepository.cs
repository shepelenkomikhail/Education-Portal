using EducationPortal.Data.Repo.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace EducationPortal.Data.Repo.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly DbContext context;

    public BaseRepository(DbContext context)
    {
        this.context = context;
    }
    
    public IQueryable<TEntity> GetAll()
    {
        return context.Set<TEntity>();
    }

    public TEntity? GetById(int id)
    {
        return context.Set<TEntity>().Find(id);
    }

    public bool Insert(TEntity entity)
    {
        try
        {
            context.Set<TEntity>().Add(entity);
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    public bool Update(TEntity entity)
    {
        try
        {
            context.Set<TEntity>().Update(entity);
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    public bool Delete(TEntity entity)
    {
        try
        {
            context.Set<TEntity>().Remove(entity);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool Delete(int id)
    {
        TEntity? entityToDelete = GetById(id);
        return entityToDelete != null && Delete(entityToDelete); 
    }
}