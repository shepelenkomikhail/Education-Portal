using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationPortal.Data.Models.EntityConfigurations;

public class VideoConfiguration : IEntityTypeConfiguration<Video>
{
    public void Configure(EntityTypeBuilder<Video> builder)
    {
        builder.HasData(
            new Video { Id = 4, Title = "C# Tutorial for Beginners", Duration = 120, Quality = 1080 },
            new Video { Id = 5, Title = "ASP.NET Core MVC Tutorial", Duration = 180, Quality = 720 },
            new Video { Id = 6, Title = "JavaScript ES6 Features", Duration = 90, Quality = 1080 }
        );
    }
}