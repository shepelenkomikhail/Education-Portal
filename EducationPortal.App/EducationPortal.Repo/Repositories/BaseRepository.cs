using EducationPortal.Data.Models;
using EducationPortal.Data.Repo.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace EducationPortal.Data.Repo.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly PortalDbContext context;

    public BaseRepository(PortalDbContext context)
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
        context.Set<TEntity>().Add(entity);
        return true;
    }
    
    public bool Update(TEntity entity)
    {
        context.Set<TEntity>().Update(entity);
        return true;
    }
    
    public bool Delete(TEntity entity)
    {
        context.Set<TEntity>().Remove(entity);
        return true;
    }

    public bool Delete(int id)
    {
        TEntity? entityToDelete = GetById(id);
        return entityToDelete != null && Delete(entityToDelete); 
    }
}