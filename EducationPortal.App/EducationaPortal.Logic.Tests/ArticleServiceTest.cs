using EducationPortal.Data.Models;
using EducationPortal.Data.Repo.RepositoryInterfaces;
using EducationPortal.Logic.DTOs;
using EducationPortal.Logic.Services;
using Moq;
using System.Linq.Expressions;

namespace EducationaPortal.Service.Tests;

public class ArticleServiceTest
{
    private readonly Mock<IUnitOfWork> mockUnitOfWork;
    private readonly Mock<IRepository<Article, int>> mockRepository;
    private readonly ArticleService articleService;

    public ArticleServiceTest()
    {
        mockUnitOfWork = new Mock<IUnitOfWork>();
        mockRepository = new Mock<IRepository<Article, int>>();
        mockUnitOfWork.Setup(u => u.Repository<Article, int>()).Returns(mockRepository.Object);
        articleService = new ArticleService(mockUnitOfWork.Object);
    }

    [Fact]
    public async Task InsertAsync_ValidArticle_ReturnsTrue()
    {
        var articleDto = new ArticleDTO
        {
            Title = "Test Article",
            Date = DateTime.UtcNow,
            Resource = "https://example.com"
        };

        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(true);

        var result = await articleService.InsertAsync(articleDto);

        Assert.True(result);
        mockRepository.Verify(r => r.InsertAsync(It.IsAny<Article>()), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task InsertAsync_SaveFails_ReturnsFalse()
    {
        var articleDto = new ArticleDTO
        {
            Title = "Test Article",
            Date = DateTime.UtcNow,
            Resource = "https://example.com"
        };

        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(false);

        var result = await articleService.InsertAsync(articleDto);

        Assert.False(result);
        mockRepository.Verify(r => r.InsertAsync(It.IsAny<Article>()), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ExistingArticle_ReturnsTrue()
    {
        var articleDto = new ArticleDTO
        {
            Id = 1,
            Title = "Updated Article",
            Date = DateTime.UtcNow,
            Resource = "https://example.com/updated"
        };

        var existingArticle = new Article
        {
            Id = 1,
            Title = "Original Article",
            Date = DateTime.UtcNow.AddDays(-1),
            Resource = "https://example.com"
        };

        mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingArticle);
        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(true);

        var result = await articleService.UpdateAsync(articleDto);

        Assert.True(result);
        Assert.Equal("Updated Article", existingArticle.Title);
        Assert.Equal(articleDto.Date, existingArticle.Date);
        Assert.Equal("https://example.com/updated", existingArticle.Resource);
        mockRepository.Verify(r => r.UpdateAsync(existingArticle), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_NonExistentArticle_ReturnsFalse()
    {
        var articleDto = new ArticleDTO
        {
            Id = 999,
            Title = "Non-existent Article",
            Date = DateTime.UtcNow,
            Resource = "https://example.com"
        };

        mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Article?)null);

        var result = await articleService.UpdateAsync(articleDto);

        Assert.False(result);
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Article>()), Times.Never);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ValidId_ReturnsTrue()
    {
        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(true);

        var result = await articleService.DeleteAsync(1);

        Assert.True(result);
        mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_SaveFails_ReturnsFalse()
    {
        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(false);

        var result = await articleService.DeleteAsync(1);

        Assert.False(result);
        mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingArticle_ReturnsArticleDto()
    {
        var article = new Article
        {
            Id = 1,
            Title = "Test Article",
            Date = DateTime.UtcNow,
            Resource = "https://example.com"
        };

        mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(article);

        var result = await articleService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Article", result.Title);
        Assert.Equal(article.Date, result.Date);
        Assert.Equal("https://example.com", result.Resource);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistentArticle_ReturnsNull()
    {
        mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Article?)null);

        var result = await articleService.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllArticles()
    {
        var articles = new List<Article>
        {
            new Article { Id = 1, Title = "Article 1", Date = DateTime.UtcNow, Resource = "https://example1.com" },
            new Article { Id = 2, Title = "Article 2", Date = DateTime.UtcNow, Resource = "https://example2.com" }
        };

        mockRepository.Setup(r => r.GetWhereAsync(
                It.IsAny<Expression<Func<Article, bool>>>()))
            .ReturnsAsync(articles);

        var result = await articleService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, a => a.Title == "Article 1");
        Assert.Contains(result, a => a.Title == "Article 2");
    }

    [Fact]
    public async Task GetAllAsync_NoArticles_ReturnsEmptyCollection()
    {
        mockRepository.Setup(r => r.GetWhereAsync(
                It.IsAny<Expression<Func<Article, bool>>>()))
            .ReturnsAsync(new List<Article>());

        var result = await articleService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}