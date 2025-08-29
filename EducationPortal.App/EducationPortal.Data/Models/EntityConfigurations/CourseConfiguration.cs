using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationPortal.Data.Models.EntityConfigurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasData(
            new Course
            {
                Id = 1, Name = "Introduction to C#",
                Description =
                    "Learn the basics of C# programming language, including syntax, data types, and control structures."
            },
            new Course
            {
                Id = 2, Name = "Web Development with ASP.NET Core",
                Description = "Build modern web applications using ASP.NET Core framework and Entity Framework."
            },
            new Course
            {
                Id = 3, Name = "JavaScript Fundamentals",
                Description = "Master JavaScript basics, DOM manipulation, and modern ES6+ features."
            },
            new Course
            {
                Id = 4, Name = "Database Design with SQL",
                Description = "Learn database design principles and SQL querying techniques."
            },
            new Course
            {
                Id = 5, Name = "React for Beginners",
                Description = "Build interactive user interfaces with React library and modern JavaScript."
            });
    }
}