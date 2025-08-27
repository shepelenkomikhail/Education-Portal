using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EducationPortal.Data.Models.EntityConfigurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasData(
            new Book { Id = 1, Title = "C# Programming Guide", Author = "Microsoft", PageAmount = 450, Formant = "PDF", PublicationDate = new DateTime(2023, 1, 15) },
            new Book { Id = 2, Title = "Entity Framework Core", Author = "Jon P Smith", PageAmount = 380, Formant = "PDF", PublicationDate = new DateTime(2023, 3, 20) },
            new Book { Id = 3, Title = "JavaScript: The Good Parts", Author = "Douglas Crockford", PageAmount = 172, Formant = "PDF", PublicationDate = new DateTime(2022, 8, 10) }
        );
    }
}