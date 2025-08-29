using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationPortal.Data.Models.EntityConfigurations;

public class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.HasData(
            new Article { Id = 7, Title = "Getting Started with Entity Framework", Date = new DateTime(2023, 6, 15), Resource = "Microsoft Docs" },
            new Article { Id = 8, Title = "Best Practices for C# Development", Date = new DateTime(2023, 5, 22), Resource = "Stack Overflow Blog" },
            new Article { Id = 9, Title = "Modern JavaScript Patterns", Date = new DateTime(2023, 4, 18), Resource = "MDN Web Docs" }
        );
    }
}