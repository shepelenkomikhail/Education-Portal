using EducationPortal.Data.Models.EntityConfigurations;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EducationPortal.Data.Models;

public class PortalDbContext : DbContext, IDataProtectionKeyContext
{
    private string connectionString;
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Skill> Skills { get; set; }
    public virtual DbSet<Course> Courses { get; set; }
    public virtual DbSet<Material> Materials { get; set; }
    public virtual DbSet<Book> Books { get; set; }
    public virtual DbSet<Video> Videos { get; set; }
    public virtual DbSet<Article> Articles { get; set; }
    public virtual DbSet<UserCourse> UserCourses { get; set; }
    public virtual DbSet<UserSkill> UserSkills { get; set; }
    public virtual DbSet<CourseSkill> CourseSkills { get; set; }
    public virtual DbSet<CourseMaterial> CourseMaterials { get; set; }

    public PortalDbContext() { }
    
    public PortalDbContext(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public PortalDbContext(DbContextOptions<PortalDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlite("DataSource=EducationPortal.db;Cache=Shared")
                .LogTo(Console.WriteLine)
                .EnableSensitiveDataLogging(true);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>().ToTable("Books");
        modelBuilder.Entity<Video>().ToTable("Videos");
        modelBuilder.Entity<Article>().ToTable("Articles");
        
        modelBuilder.Entity<UserCourse>().HasKey(uc => new { uc.UserId, uc.CourseId });
        modelBuilder.Entity<UserSkill>().HasKey(uc => new { uc.UserId, uc.SkillId });
        modelBuilder.Entity<CourseSkill>().HasKey(cs => new { cs.CourseId, cs.SkillId });
        modelBuilder.Entity<CourseMaterial>().HasKey(cm => new { cm.CourseId, cm.MaterialId });
        
        modelBuilder.ApplyConfiguration(new SkillConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new CourseConfiguration());
        modelBuilder.ApplyConfiguration(new BookConfiguration());
        modelBuilder.ApplyConfiguration(new VideoConfiguration());
        modelBuilder.ApplyConfiguration(new ArticleConfiguration());
        modelBuilder.ApplyConfiguration(new UserCourseConfiguration());
        modelBuilder.ApplyConfiguration(new UserSkillConfiguration());
    }
}