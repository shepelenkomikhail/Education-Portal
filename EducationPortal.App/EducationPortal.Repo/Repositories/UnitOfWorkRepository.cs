using EducationPortal.Data.Models;
using EducationPortal.Data.Repo.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace EducationPortal.Data.Repo.Repositories;

public class UnitOfWorkRepository : IUnitOfWorkRepository
{
    private readonly PortalDbContext context;
    public CourseRepository Courses { get; }
    // TODO: add other repositories
    
    public UnitOfWorkRepository(PortalDbContext context)
    {
        this.context = context;
        Courses = new CourseRepository(context);
        // TODO: add other repositories
    }

    public bool Save()
    {
        try
        {
            context.SaveChanges();
            return true; 
        }
        catch (DbUpdateException ex)
        {
            foreach (var entry in ex.Entries)
            {
                entry.State = EntityState.Detached;
            }

            return false;
        }
    }
    public void Dispose() => context.Dispose();
}