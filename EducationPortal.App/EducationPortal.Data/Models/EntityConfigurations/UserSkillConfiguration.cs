using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationPortal.Data.Models.EntityConfigurations;

public class UserSkillConfiguration : IEntityTypeConfiguration<UserSkill>
{
    public void Configure(EntityTypeBuilder<UserSkill> builder)
    {
        builder.HasData(
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
        );
    }
}