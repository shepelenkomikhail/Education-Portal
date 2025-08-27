using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationPortal.Data.Models.EntityConfigurations;

public class SkillConfiguration : IEntityTypeConfiguration<Skill>
{
    public void Configure(EntityTypeBuilder<Skill> builder)
    {
        builder.HasData(
            new Skill { Id = 1, Name = "C#" },
            new Skill { Id = 2, Name = "JavaScript" },
            new Skill { Id = 3, Name = "Python" },
            new Skill { Id = 4, Name = "SQL" },
            new Skill { Id = 5, Name = "HTML/CSS" },
            new Skill { Id = 6, Name = "React" },
            new Skill { Id = 7, Name = "Entity Framework" },
            new Skill { Id = 8, Name = "ASP.NET Core" }
        );
    }
}