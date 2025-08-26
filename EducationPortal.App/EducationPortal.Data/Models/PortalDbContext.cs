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
    
    public PortalDbContext() { }

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
        modelBuilder.Entity<Book>().ToTable("Books");
        modelBuilder.Entity<Video>().ToTable("Videos");
        modelBuilder.Entity<Article>().ToTable("Articles");
        
        modelBuilder.Entity<UserCourse>().HasKey(uc => new { uc.UserId, uc.CourseId });
        modelBuilder.Entity<UserSkill>().HasKey(uc => new { uc.UserId, uc.SkillId });
        
        var skills = new List<Skill>
        {
            new Skill { Id = 1, Name = "C#" },
            new Skill { Id = 2, Name = "JavaScript" },
            new Skill { Id = 3, Name = "Python" },
            new Skill { Id = 4, Name = "SQL" },
            new Skill { Id = 5, Name = "HTML/CSS" },
            new Skill { Id = 6, Name = "React" },
            new Skill { Id = 7, Name = "Entity Framework" },
            new Skill { Id = 8, Name = "ASP.NET Core" }
        };
        
        var users = new List<User>
        {
            new User { Id = 1, Name = "John", Surname = "Doe", Email = "john.doe@email.com", Password = "password123", Phone = "+1234567890" },
            new User { Id = 2, Name = "Jane", Surname = "Smith", Email = "jane.smith@email.com", Password = "password123", Phone = "+1234567891" },
            new User { Id = 3, Name = "Bob", Surname = "Johnson", Email = "bob.johnson@email.com", Password = "password123", Phone = "+1234567892" },
            new User { Id = 4, Name = "Alice", Surname = "Brown", Email = "alice.brown@email.com", Password = "password123", Phone = "+1234567893" },
            new User { Id = 5, Name = "Charlie", Surname = "Wilson", Email = "charlie.wilson@email.com", Password = "password123", Phone = "+1234567894" }
        };
        
        var courses = new List<Course>
        {
            new Course { Id = 1, Name = "Introduction to C#", Description = "Learn the basics of C# programming language, including syntax, data types, and control structures." },
            new Course { Id = 2, Name = "Web Development with ASP.NET Core", Description = "Build modern web applications using ASP.NET Core framework and Entity Framework." },
            new Course { Id = 3, Name = "JavaScript Fundamentals", Description = "Master JavaScript basics, DOM manipulation, and modern ES6+ features." },
            new Course { Id = 4, Name = "Database Design with SQL", Description = "Learn database design principles and SQL querying techniques." },
            new Course { Id = 5, Name = "React for Beginners", Description = "Build interactive user interfaces with React library and modern JavaScript." }
        };
        
        var books = new List<Book>
        {
            new Book { Id = 1, Title = "C# Programming Guide", Author = "Microsoft", PageAmount = 450, Formant = "PDF", PublicationDate = new DateTime(2023, 1, 15) },
            new Book { Id = 2, Title = "Entity Framework Core", Author = "Jon P Smith", PageAmount = 380, Formant = "PDF", PublicationDate = new DateTime(2023, 3, 20) },
            new Book { Id = 3, Title = "JavaScript: The Good Parts", Author = "Douglas Crockford", PageAmount = 172, Formant = "PDF", PublicationDate = new DateTime(2022, 8, 10) }
        };
        
        var videos = new List<Video>
        {
            new Video { Id = 4, Title = "C# Tutorial for Beginners", Duration = 120, Quality = 1080 },
            new Video { Id = 5, Title = "ASP.NET Core MVC Tutorial", Duration = 180, Quality = 720 },
            new Video { Id = 6, Title = "JavaScript ES6 Features", Duration = 90, Quality = 1080 }
        };
        
        var articles = new List<Article>
        {
            new Article { Id = 7, Title = "Getting Started with Entity Framework", Date = new DateTime(2023, 6, 15), Resource = "Microsoft Docs" },
            new Article { Id = 8, Title = "Best Practices for C# Development", Date = new DateTime(2023, 5, 22), Resource = "Stack Overflow Blog" },
            new Article { Id = 9, Title = "Modern JavaScript Patterns", Date = new DateTime(2023, 4, 18), Resource = "MDN Web Docs" }
        };
        
        var userCourses = new List<UserCourse>
        {
            new UserCourse { UserId = 1, CourseId = 1 },
            new UserCourse { UserId = 1, CourseId = 2 },
            new UserCourse { UserId = 2, CourseId = 1 },
            new UserCourse { UserId = 2, CourseId = 3 },
            new UserCourse { UserId = 3, CourseId = 2 },
            new UserCourse { UserId = 3, CourseId = 4 },
            new UserCourse { UserId = 4, CourseId = 3 },
            new UserCourse { UserId = 4, CourseId = 5 },
            new UserCourse { UserId = 5, CourseId = 1 },
            new UserCourse { UserId = 5, CourseId = 5 }
        };
        
        var userSkills = new List<UserSkill>
        {
            new UserSkill { UserId = 1, SkillId = 1 },
            new UserSkill { UserId = 1, SkillId = 7 },
            new UserSkill { UserId = 2, SkillId = 3 },
            new UserSkill { UserId = 2, SkillId = 5 },
            new UserSkill { UserId = 3, SkillId = 2 },
            new UserSkill { UserId = 3, SkillId = 4 },
            new UserSkill { UserId = 4, SkillId = 6 },
            new UserSkill { UserId = 4, SkillId = 5 },
            new UserSkill { UserId = 5, SkillId = 1 },
            new UserSkill { UserId = 5, SkillId = 8 } 
        };
        
        modelBuilder.Entity<Skill>().HasData(skills);
        modelBuilder.Entity<User>().HasData(users);
        modelBuilder.Entity<Course>().HasData(courses);
        modelBuilder.Entity<Book>().HasData(books);
        modelBuilder.Entity<Video>().HasData(videos);
        modelBuilder.Entity<Article>().HasData(articles);
        modelBuilder.Entity<UserCourse>().HasData(userCourses);
        modelBuilder.Entity<UserSkill>().HasData(userSkills);
    }
}