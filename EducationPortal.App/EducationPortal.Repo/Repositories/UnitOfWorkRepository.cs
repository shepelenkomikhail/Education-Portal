using EducationPortal.Data.Models;
using EducationPortal.Data.Repo.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EducationPortal.Data.Repo.Repositories;

public class UnitOfWorkRepository : IUnitOfWorkRepository
{
    private readonly PortalDbContext context;
    public CourseRepository Courses { get; }
    public SkillRepository Skills { get; }
    public UserRepository Users { get; }
    public MaterialRepository Materials { get; }
    public ArticleRepository Articles { get; }
    public BookRepository Books { get; }
    public VideoRepository Videos { get; }
    
    public UnitOfWorkRepository(PortalDbContext context)
    {
        this.context = context;
        Courses = new CourseRepository(context);
        Skills = new SkillRepository(context);
        Users = new UserRepository(context);
        Materials = new MaterialRepository(context);
        Articles = new ArticleRepository(context);
        Books = new BookRepository(context);
        Videos = new VideoRepository(context);
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
    
    public IDbContextTransaction BeginTransaction()
    {
        return context.Database.BeginTransaction();
    }
    
    public void Dispose() => context.Dispose();
}