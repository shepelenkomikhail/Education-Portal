using EducationPortal.Data.Models;

namespace EducationPortal.Logic.DTOs;

public class BookDTO : MaterialDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int PageAmount { get; set; }
    public string Formant { get; set; } = string.Empty;
    public DateTime PublicationDate { get; set; }
    
    public BookDTO(){}

    internal BookDTO(Book? book)
    {
        ArgumentNullException.ThrowIfNull(book, nameof(book));
        
        Id = book.Id;
        Title = book.Title;
        Author = book.Author;
        PageAmount = book.PageAmount;
        Formant = book.Formant;
        PublicationDate = book.PublicationDate;
    }

    internal Book ToBook()
    {
        return new Book() 
        { 
            Id = this.Id, 
            Title = this.Title, 
            Author = this.Author, 
            PageAmount = this.PageAmount, 
            Formant = this.Formant, 
            PublicationDate = this.PublicationDate 
        };
    }
}