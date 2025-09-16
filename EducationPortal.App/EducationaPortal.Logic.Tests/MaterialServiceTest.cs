using System.Linq.Expressions;
using EducationPortal.Data.Models;
using EducationPortal.Data.Repo.RepositoryInterfaces;
using EducationPortal.Logic.DTOs;
using EducationPortal.Logic.Services;
using Moq;

namespace EducationaPortal.Service.Tests;

public class MaterialServiceTest
{
    private readonly Mock<IUnitOfWork> mockUnitOfWork;
    private readonly Mock<IRepository<Book, int>> mockBookRepository;
    private readonly Mock<IRepository<Video, int>> mockVideoRepository;
    private readonly Mock<IRepository<Article, int>> mockArticleRepository;
    private readonly MaterialService materialService;

    public MaterialServiceTest()
    {
        mockUnitOfWork = new Mock<IUnitOfWork>();
        mockBookRepository = new Mock<IRepository<Book, int>>();
        mockVideoRepository = new Mock<IRepository<Video, int>>();
        mockArticleRepository = new Mock<IRepository<Article, int>>();
        
        mockUnitOfWork.Setup(u => u.Repository<Book, int>()).Returns(mockBookRepository.Object);
        mockUnitOfWork.Setup(u => u.Repository<Video, int>()).Returns(mockVideoRepository.Object);
        mockUnitOfWork.Setup(u => u.Repository<Article, int>()).Returns(mockArticleRepository.Object);
        
        materialService = new MaterialService(mockUnitOfWork.Object);
    }

    [Fact]
    public async Task InsertAsync_BookMaterial_ReturnsTrue()
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

        var result = await materialService.InsertAsync(bookDto);

        Assert.True(result);
        mockBookRepository.Verify(r => r.InsertAsync(It.IsAny<Book>()), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task InsertAsync_VideoMaterial_ReturnsTrue()
    {
        var videoDto = new VideoDTO
        {
            Title = "Test Video",
            Duration = 120,
            Quality = 1080
        };

        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(true);

        var result = await materialService.InsertAsync(videoDto);

        Assert.True(result);
        mockVideoRepository.Verify(r => r.InsertAsync(It.IsAny<Video>()), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task InsertAsync_ArticleMaterial_ReturnsTrue()
    {
        var articleDto = new ArticleDTO
        {
            Title = "Test Article",
            Date = DateTime.UtcNow,
            Resource = "https://example.com"
        };

        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(true);

        var result = await materialService.InsertAsync(articleDto);

        Assert.True(result);
        mockArticleRepository.Verify(r => r.InsertAsync(It.IsAny<Article>()), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task InsertAsync_InvalidMaterial_ReturnsFalse()
    {
        var materialDto = new MaterialDTO
        {
            Title = "Invalid Material"
        };

        var result = await materialService.InsertAsync(materialDto);

        Assert.False(result);
        mockBookRepository.Verify(r => r.InsertAsync(It.IsAny<Book>()), Times.Never);
        mockVideoRepository.Verify(r => r.InsertAsync(It.IsAny<Video>()), Times.Never);
        mockArticleRepository.Verify(r => r.InsertAsync(It.IsAny<Article>()), Times.Never);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_BookMaterial_ReturnsTrue()
    {
        var bookDto = new BookDTO
        {
            Id = 1,
            Title = "Updated Book"
        };

        var existingBook = new Book
        {
            Id = 1,
            Title = "Original Book",
            Author = "Test Author",
            PageAmount = 300,
            Formant = "PDF",
            PublicationDate = DateTime.UtcNow
        };

        mockBookRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingBook);
        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(true);

        var result = await materialService.UpdateAsync(bookDto);

        Assert.True(result);
        Assert.Equal("Updated Book", existingBook.Title);
        mockBookRepository.Verify(r => r.UpdateAsync(existingBook), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_VideoMaterial_ReturnsTrue()
    {
        var videoDto = new VideoDTO
        {
            Id = 1,
            Title = "Updated Video"
        };

        var existingVideo = new Video
        {
            Id = 1,
            Title = "Original Video",
            Duration = 120,
            Quality = 1080
        };

        mockVideoRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingVideo);
        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(true);

        var result = await materialService.UpdateAsync(videoDto);

        Assert.True(result);
        Assert.Equal("Updated Video", existingVideo.Title);
        mockVideoRepository.Verify(r => r.UpdateAsync(existingVideo), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ArticleMaterial_ReturnsTrue()
    {
        var articleDto = new ArticleDTO
        {
            Id = 1,
            Title = "Updated Article"
        };

        var existingArticle = new Article
        {
            Id = 1,
            Title = "Original Article",
            Date = DateTime.UtcNow,
            Resource = "https://example.com"
        };

        mockArticleRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingArticle);
        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(true);

        var result = await materialService.UpdateAsync(articleDto);

        Assert.True(result);
        Assert.Equal("Updated Article", existingArticle.Title);
        mockArticleRepository.Verify(r => r.UpdateAsync(existingArticle), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_NonExistentMaterial_ReturnsFalse()
    {
        var materialDto = new MaterialDTO
        {
            Id = 999,
            Title = "Non-existent Material"
        };

        mockBookRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Book?)null);
        mockVideoRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Video?)null);
        mockArticleRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Article?)null);

        var result = await materialService.UpdateAsync(materialDto);

        Assert.False(result);
        mockBookRepository.Verify(r => r.UpdateAsync(It.IsAny<Book>()), Times.Never);
        mockVideoRepository.Verify(r => r.UpdateAsync(It.IsAny<Video>()), Times.Never);
        mockArticleRepository.Verify(r => r.UpdateAsync(It.IsAny<Article>()), Times.Never);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_BookMaterial_ReturnsTrue()
    {
        var existingBook = new Book
        {
            Id = 1,
            Title = "Test Book",
            Author = "Test Author",
            PageAmount = 300,
            Formant = "PDF",
            PublicationDate = DateTime.UtcNow
        };

        mockBookRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingBook);
        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(true);

        var result = await materialService.DeleteAsync(1);

        Assert.True(result);
        mockBookRepository.Verify(r => r.DeleteAsync(existingBook), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_VideoMaterial_ReturnsTrue()
    {
        var existingVideo = new Video
        {
            Id = 1,
            Title = "Test Video",
            Duration = 120,
            Quality = 1080
        };

        mockVideoRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingVideo);
        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(true);

        var result = await materialService.DeleteAsync(1);

        Assert.True(result);
        mockVideoRepository.Verify(r => r.DeleteAsync(existingVideo), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ArticleMaterial_ReturnsTrue()
    {
        var existingArticle = new Article
        {
            Id = 1,
            Title = "Test Article",
            Date = DateTime.UtcNow,
            Resource = "https://example.com"
        };

        mockArticleRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingArticle);
        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(true);

        var result = await materialService.DeleteAsync(1);

        Assert.True(result);
        mockArticleRepository.Verify(r => r.DeleteAsync(existingArticle), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_NonExistentMaterial_ReturnsFalse()
    {
        mockBookRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Book?)null);
        mockVideoRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Video?)null);
        mockArticleRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Article?)null);

        var result = await materialService.DeleteAsync(999);

        Assert.False(result);
        mockBookRepository.Verify(r => r.DeleteAsync(It.IsAny<Book>()), Times.Never);
        mockVideoRepository.Verify(r => r.DeleteAsync(It.IsAny<Video>()), Times.Never);
        mockArticleRepository.Verify(r => r.DeleteAsync(It.IsAny<Article>()), Times.Never);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task GetByIdAsync_BookMaterial_ReturnsBookDto()
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

        mockBookRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(book);

        var result = await materialService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.IsType<BookDTO>(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Book", result.Title);
    }

    [Fact]
    public async Task GetByIdAsync_VideoMaterial_ReturnsVideoDto()
    {
        var video = new Video
        {
            Id = 1,
            Title = "Test Video",
            Duration = 120,
            Quality = 1080
        };

        mockVideoRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(video);

        var result = await materialService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.IsType<VideoDTO>(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Video", result.Title);
    }

    [Fact]
    public async Task GetByIdAsync_ArticleMaterial_ReturnsArticleDto()
    {
        var article = new Article
        {
            Id = 1,
            Title = "Test Article",
            Date = DateTime.UtcNow,
            Resource = "https://example.com"
        };

        mockArticleRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(article);

        var result = await materialService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.IsType<ArticleDTO>(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Article", result.Title);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistentMaterial_ReturnsNull()
    {
        mockBookRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Book?)null);
        mockVideoRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Video?)null);
        mockArticleRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Article?)null);

        var result = await materialService.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByTitleAsync_BookMaterial_ReturnsBookDto()
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

        mockBookRepository.Setup(r => r.GetSingleOrDefaultAsync(It.IsAny<Expression<Func<Book, bool>>>()))
            .ReturnsAsync(book);

        var result = await materialService.GetByTitleAsync("Test Book");

        Assert.NotNull(result);
        Assert.IsType<BookDTO>(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Book", result.Title);
    }

    [Fact]
    public async Task GetByTitleAsync_NonExistentMaterial_ReturnsNull()
    {
        mockBookRepository.Setup(r => r.GetSingleOrDefaultAsync(It.IsAny<Expression<Func<Book, bool>>>()))
            .ReturnsAsync((Book?)null);
        mockVideoRepository.Setup(r => r.GetSingleOrDefaultAsync(It.IsAny<Expression<Func<Video, bool>>>()))
            .ReturnsAsync((Video?)null);
        mockArticleRepository.Setup(r => r.GetSingleOrDefaultAsync(It.IsAny<Expression<Func<Article, bool>>>()))
            .ReturnsAsync((Article?)null);

        var result = await materialService.GetByTitleAsync("Non-existent Material");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllMaterials()
    {
        var books = new List<Book>
        {
            new Book { Id = 1, Title = "Book 1", Author = "Author 1", PageAmount = 300, Formant = "PDF", PublicationDate = DateTime.UtcNow }
        };
        var videos = new List<Video>
        {
            new Video { Id = 2, Title = "Video 1", Duration = 120, Quality = 1080 }
        };
        var articles = new List<Article>
        {
            new Article { Id = 3, Title = "Article 1", Date = DateTime.UtcNow, Resource = "https://example.com" }
        };

        mockBookRepository.Setup(r => r.GetWhereAsync(It.IsAny<Expression<Func<Book, bool>>>()))
            .ReturnsAsync(books);
        mockVideoRepository.Setup(r => r.GetWhereAsync(It.IsAny<Expression<Func<Video, bool>>>()))
            .ReturnsAsync(videos);
        mockArticleRepository.Setup(r => r.GetWhereAsync(It.IsAny<Expression<Func<Article, bool>>>()))
            .ReturnsAsync(articles);

        var result = await materialService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
        Assert.Contains(result, m => m.Title == "Book 1");
        Assert.Contains(result, m => m.Title == "Video 1");
        Assert.Contains(result, m => m.Title == "Article 1");
    }

    [Fact]
    public async Task GetByTypeAsync_BookType_ReturnsBooks()
    {
        var books = new List<Book>
        {
            new Book { Id = 1, Title = "Book 1", Author = "Author 1", PageAmount = 300, Formant = "PDF", PublicationDate = DateTime.UtcNow }
        };

        mockBookRepository.Setup(r => r.GetWhereAsync(
                It.IsAny<Expression<Func<Book, bool>>>()))
            .ReturnsAsync(books);

        var result = await materialService.GetByTypeAsync("book");

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.IsType<BookDTO>(result.First());
        Assert.Equal("Book 1", result.First().Title);
    }

    [Fact]
    public async Task GetByTypeAsync_VideoType_ReturnsVideos()
    {
        var videos = new List<Video>
        {
            new Video { Id = 1, Title = "Video 1", Duration = 120, Quality = 1080 }
        };

        mockVideoRepository.Setup(r => r.GetWhereAsync(
                It.IsAny<Expression<Func<Video, bool>>>()))
            .ReturnsAsync(videos);

        var result = await materialService.GetByTypeAsync("video");

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.IsType<VideoDTO>(result.First());
        Assert.Equal("Video 1", result.First().Title);
    }

    [Fact]
    public async Task GetByTypeAsync_ArticleType_ReturnsArticles()
    {
        var articles = new List<Article>
        {
            new Article { Id = 1, Title = "Article 1", Date = DateTime.UtcNow, Resource = "https://example.com" }
        };

        mockArticleRepository.Setup(r => r.GetWhereAsync(
                It.IsAny<Expression<Func<Article, bool>>>()))
            .ReturnsAsync(articles);

        var result = await materialService.GetByTypeAsync("article");

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.IsType<ArticleDTO>(result.First());
        Assert.Equal("Article 1", result.First().Title);
    }

    [Fact]
    public async Task GetByTypeAsync_InvalidType_ReturnsEmptyCollection()
    {
        var result = await materialService.GetByTypeAsync("invalid");

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
