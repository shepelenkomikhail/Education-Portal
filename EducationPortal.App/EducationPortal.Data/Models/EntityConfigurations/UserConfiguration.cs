using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationPortal.Data.Models.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasData(
            new User { Id = 2, UserName = "Jane", FirstName = "Jane", Surname = "Smith", Email = "jane.smith@email.com", PasswordHash = "password123", PhoneNumber = "+1234567891" },
            new User { Id = 3, UserName = "Bob", FirstName = "Bob", Surname = "Johnson", Email = "bob.johnson@email.com", PasswordHash = "password123", PhoneNumber = "+1234567892" },
            new User { Id = 4, UserName = "Alice", FirstName = "Alice", Surname = "Brown", Email = "alice.brown@email.com", PasswordHash = "password123", PhoneNumber = "+1234567893" },
            new User { Id = 5, UserName = "Charlie", FirstName = "Charlie", Surname = "Wilson", Email = "charlie.wilson@email.com", PasswordHash = "password123", PhoneNumber = "+1234567894" }
        );
    }
}