using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationPortal.Data.Models.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasData(
            new User { Id = 1, Name = "John", Surname = "Doe", Email = "john.doe@email.com", Password = "password123", Phone = "+1234567890" },
            new User { Id = 2, Name = "Jane", Surname = "Smith", Email = "jane.smith@email.com", Password = "password123", Phone = "+1234567891" },
            new User { Id = 3, Name = "Bob", Surname = "Johnson", Email = "bob.johnson@email.com", Password = "password123", Phone = "+1234567892" },
            new User { Id = 4, Name = "Alice", Surname = "Brown", Email = "alice.brown@email.com", Password = "password123", Phone = "+1234567893" },
            new User { Id = 5, Name = "Charlie", Surname = "Wilson", Email = "charlie.wilson@email.com", Password = "password123", Phone = "+1234567894" }
        );
    }
}