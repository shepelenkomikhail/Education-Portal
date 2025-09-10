using EducationPortal.Data.Models;
using EducationPortal.Data.Repo.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace EducationPortal.Data.Repo.Repositories;

public class RepositoryFactory : IRepositoryFactory
{
    private readonly PortalDbContext context;

    public RepositoryFactory(PortalDbContext context)
    {
        this.context = context;
    }

    public IRepository<T, TId> GetRepository<T, TId>() where T : class, IBaseEntity<TId>
    {
        return new Repository<T, TId>(context);
    }
}
