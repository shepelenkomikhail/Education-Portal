using Microsoft.EntityFrameworkCore;

namespace EducationPortal.Data.Models;

public class PortalDbContext : DbContext
{
    private string connectionString;
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Skill> Skills { get; set; }
    public virtual DbSet<Course> Courses { get; set; }
    public virtual DbSet<Material> Materials { get; set; }
    public virtual DbSet<UserCourse> UserCourses { get; set; }
    public virtual DbSet<UserSkill> UserSkills { get; set; }

    public PortalDbContext(string connectionString)
    {
        this.connectionString = connectionString;
        this.Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlite(connectionString)
                .LogTo(Console.WriteLine)
                .EnableSensitiveDataLogging(true);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Mock data to be added here
    }
}