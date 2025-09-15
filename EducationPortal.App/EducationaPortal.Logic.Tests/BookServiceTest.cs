using System.Linq.Expressions;
using EducationPortal.Data.Models;
using EducationPortal.Data.Repo.RepositoryInterfaces;
using EducationPortal.Logic.DTOs;
using EducationPortal.Logic.Services;
using Moq;

namespace EducationaPortal.Service.Tests;

public class BookServiceTest
{
    private readonly Mock<IUnitOfWork> mockUnitOfWork;
    private readonly Mock<IRepository<Book, int>> mockRepository;
    private readonly BookService bookService;

    public BookServiceTest() 
    {
        mockUnitOfWork = new Mock<IUnitOfWork>();
        mockRepository = new Mock<IRepository<Book, int>>();
        mockUnitOfWork.Setup(u => u.Repository<Book, int>()).Returns(mockRepository.Object);
        bookService = new BookService(mockUnitOfWork.Object);
    }

    [Fact]
    public async Task InsertAsync_ValidBook_ReturnsTrue()
    {
        var bookDto = new BookDTO
        {
            Title = "Test Book",
            Author = "Test Author",
            PageAmount = 300,
            Formant = "PDF",
            PublicationDate = DateTime.UtcNow
        };

        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(true);

        var result = await bookService.InsertAsync(bookDto);

        Assert.True(result);
        mockRepository.Verify(r => r.InsertAsync(It.IsAny<Book>()), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task InsertAsync_SaveFails_ReturnsFalse()
    {
        var bookDto = new BookDTO
        {
            Title = "Test Book",
            Author = "Test Author",
            PageAmount = 300,
            Formant = "PDF",
            PublicationDate = DateTime.UtcNow
        };

        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(false);

        var result = await bookService.InsertAsync(bookDto);

        Assert.False(result);
        mockRepository.Verify(r => r.InsertAsync(It.IsAny<Book>()), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ExistingBook_ReturnsTrue()
    {
        var bookDto = new BookDTO
        {
            Id = 1,
            Title = "Updated Book",
            Author = "Updated Author",
            PageAmount = 400,
            Formant = "EPUB",
            PublicationDate = DateTime.UtcNow
        };

        var existingBook = new Book
        {
            Id = 1,
            Title = "Original Book",
            Author = "Original Author",
            PageAmount = 300,
            Formant = "PDF",
            PublicationDate = DateTime.UtcNow.AddDays(-1)
        };

        mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingBook);
        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(true);

        var result = await bookService.UpdateAsync(bookDto);

        Assert.True(result);
        Assert.Equal("Updated Book", existingBook.Title);
        Assert.Equal("Updated Author", existingBook.Author);
        Assert.Equal(400, existingBook.PageAmount);
        Assert.Equal("EPUB", existingBook.Formant);
        Assert.Equal(bookDto.PublicationDate, existingBook.PublicationDate);
        mockRepository.Verify(r => r.UpdateAsync(existingBook), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_NonExistentBook_ReturnsFalse()
    {
        var bookDto = new BookDTO
        {
            Id = 999,
            Title = "Non-existent Book",
            Author = "Test Author",
            PageAmount = 300,
            Formant = "PDF",
            PublicationDate = DateTime.UtcNow
        };

        mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Book?)null);

        var result = await bookService.UpdateAsync(bookDto);

        Assert.False(result);
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Book>()), Times.Never);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ValidId_ReturnsTrue()
    {
        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(true);

        var result = await bookService.DeleteAsync(1);

        Assert.True(result);
        mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_SaveFails_ReturnsFalse()
    {
        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(false);

        var result = await bookService.DeleteAsync(1);

        Assert.False(result);
        mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingBook_ReturnsBookDto()
    {
        var book = new Book
        {
            Id = 1,
            Title = "Test Book",
            Author = "Test Author",
            PageAmount = 300,
            Formant = "PDF",
            PublicationDate = DateTime.UtcNow
        };

        mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(book);

        var result = await bookService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Book", result.Title);
        Assert.Equal("Test Author", result.Author);
        Assert.Equal(300, result.PageAmount);
        Assert.Equal("PDF", result.Formant);
        Assert.Equal(book.PublicationDate, result.PublicationDate);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistentBook_ReturnsNull()
    {
        mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Book?)null);

        var result = await bookService.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllBooks()
    {
        var books = new List<Book>
        {
            new Book { Id = 1, Title = "Book 1", Author = "Author 1", PageAmount = 300, Formant = "PDF", PublicationDate = DateTime.UtcNow },
            new Book { Id = 2, Title = "Book 2", Author = "Author 2", PageAmount = 400, Formant = "EPUB", PublicationDate = DateTime.UtcNow }
        };

        mockRepository.Setup(r => r.GetWhereAsync(
                It.IsAny<Expression<Func<Book, bool>>>()))
            .ReturnsAsync(books);

        var result = await bookService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, b => b.Title == "Book 1");
        Assert.Contains(result, b => b.Title == "Book 2");
    }

    [Fact]
    public async Task GetAllAsync_NoBooks_ReturnsEmptyCollection()
    {
        mockRepository.Setup(r => r.GetWhereAsync(
                It.IsAny<Expression<Func<Book, bool>>>()))
            .ReturnsAsync(new List<Book>());

        var result = await bookService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByAuthorAsync_ExistingAuthor_ReturnsBooks()
    {
        var books = new List<Book>
        {
            new Book { Id = 1, Title = "Book 1", Author = "John Doe", PageAmount = 300, Formant = "PDF", PublicationDate = DateTime.UtcNow },
            new Book { Id = 2, Title = "Book 2", Author = "John Smith", PageAmount = 400, Formant = "EPUB", PublicationDate = DateTime.UtcNow }
        };

        mockRepository.Setup(r => r.GetWhereAsync(
                It.IsAny<Expression<Func<Book, bool>>>()))
            .ReturnsAsync(books);

        var result = await bookService.GetByAuthorAsync("John");

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.All(result, b => Assert.Contains("John", b.Author));
    }

    [Fact]
    public async Task GetByAuthorAsync_NonExistentAuthor_ReturnsEmptyCollection()
    {
        mockRepository.Setup(r => r.GetWhereAsync(
                It.IsAny<Expression<Func<Book, bool>>>()))
            .ReturnsAsync(new List<Book>());

        var result = await bookService.GetByAuthorAsync("NonExistentAuthor");

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}