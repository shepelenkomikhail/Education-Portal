using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationPortal.Data.Models.EntityConfigurations;

public class UserCourseConfiguration : IEntityTypeConfiguration<UserCourse>
{
    public void Configure(EntityTypeBuilder<UserCourse> builder)
    {
        builder.HasData(
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
        );
    }
}