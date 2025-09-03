using EducationPortal.Data.Models;
using EducationPortal.Data.Repo.RepositoryInterfaces;
using EducationPortal.Logic.DTOs;
using EducationPortal.Logic.Interfaces;

namespace EducationPortal.Logic.Services;

public class BookService: IBookService
{
    private readonly IUnitOfWork unitOfWork;
    
    public BookService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }
    
    public async Task<bool> InsertAsync(BookDTO book)
    {
        var bookEntity = new Book
        {
            Title = book.Title,
            Author = book.Author,
            PageAmount = book.PageAmount,
            Formant = book.Formant,
            PublicationDate = book.PublicationDate
        };
        await unitOfWork.Repository<Book, int>().InsertAsync(bookEntity);
        return await unitOfWork.SaveAsync();
    }

    public async Task<bool> UpdateAsync(BookDTO book)
    {
        var existingBook = await unitOfWork.Repository<Book, int>().GetByIdAsync(book.Id);
        if (existingBook == null) return false;
        
        existingBook.Title = book.Title;
        existingBook.Author = book.Author;
        existingBook.PageAmount = book.PageAmount;
        existingBook.Formant = book.Formant;
        existingBook.PublicationDate = book.PublicationDate;
        await unitOfWork.Repository<Book, int>().UpdateAsync(existingBook);
        return await unitOfWork.SaveAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await unitOfWork.Repository<Book, int>().DeleteAsync(id);
        return await unitOfWork.SaveAsync();
    }

    public async Task<BookDTO?> GetByIdAsync(int id)
    {
        var book = await unitOfWork.Repository<Book, int>().GetByIdAsync(id);
        return book != null ? new BookDTO(book) : null;
    }

    public async Task<IEnumerable<BookDTO>> GetAllAsync()
    {
        var books = await unitOfWork.Repository<Book, int>().GetWhereAsync(b => true);
        return books.Select(b => new BookDTO(b)).ToList();
    }

    public async Task<IEnumerable<BookDTO>> GetByAuthorAsync(string author)
    {
        var books = await unitOfWork.Repository<Book, int>().GetWhereAsync(b => b.Author.Contains(author));
        return books.Select(b => new BookDTO(b)).ToList();
    }
}