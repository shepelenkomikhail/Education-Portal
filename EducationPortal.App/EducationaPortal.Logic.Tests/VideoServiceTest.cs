using System.Linq.Expressions;
using EducationPortal.Data.Models;
using EducationPortal.Data.Repo.RepositoryInterfaces;
using EducationPortal.Logic.DTOs;
using EducationPortal.Logic.Services;
using Moq;

namespace EducationaPortal.Service.Tests;

public class VideoServiceTest
{
    private readonly Mock<IUnitOfWork> mockUnitOfWork;
    private readonly Mock<IRepository<Video, int>> mockRepository;
    private readonly VideoService videoService;

    public VideoServiceTest()
    {
        mockUnitOfWork = new Mock<IUnitOfWork>();
        mockRepository = new Mock<IRepository<Video, int>>();
        mockUnitOfWork.Setup(u => u.Repository<Video, int>()).Returns(mockRepository.Object);
        videoService = new VideoService(mockUnitOfWork.Object);
    }

    [Fact]
    public async Task InsertAsync_ValidVideo_ReturnsTrue()
    {
        var videoDto = new VideoDTO
        {
            Title = "Test Video",
            Duration = 120,
            Quality = 1080
        };

        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(true);

        var result = await videoService.InsertAsync(videoDto);

        Assert.True(result);
        mockRepository.Verify(r => r.InsertAsync(It.IsAny<Video>()), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task InsertAsync_SaveFails_ReturnsFalse()
    {
        var videoDto = new VideoDTO
        {
            Title = "Test Video",
            Duration = 120,
            Quality = 1080
        };

        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(false);

        var result = await videoService.InsertAsync(videoDto);

        Assert.False(result);
        mockRepository.Verify(r => r.InsertAsync(It.IsAny<Video>()), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ExistingVideo_ReturnsTrue()
    {
        var videoDto = new VideoDTO
        {
            Id = 1,
            Title = "Updated Video",
            Duration = 180,
            Quality = 720
        };

        var existingVideo = new Video
        {
            Id = 1,
            Title = "Original Video",
            Duration = 120,
            Quality = 1080
        };

        mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingVideo);
        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(true);

        var result = await videoService.UpdateAsync(videoDto);

        Assert.True(result);
        Assert.Equal("Updated Video", existingVideo.Title);
        Assert.Equal(180, existingVideo.Duration);
        Assert.Equal(720, existingVideo.Quality);
        mockRepository.Verify(r => r.UpdateAsync(existingVideo), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_NonExistentVideo_ReturnsFalse()
    {
        var videoDto = new VideoDTO
        {
            Id = 999,
            Title = "Non-existent Video",
            Duration = 120,
            Quality = 1080
        };

        mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Video?)null);

        var result = await videoService.UpdateAsync(videoDto);

        Assert.False(result);
        mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Video>()), Times.Never);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ValidId_ReturnsTrue()
    {
        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(true);

        var result = await videoService.DeleteAsync(1);

        Assert.True(result);
        mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_SaveFails_ReturnsFalse()
    {
        mockUnitOfWork.Setup(u => u.SaveAsync()).ReturnsAsync(false);

        var result = await videoService.DeleteAsync(1);

        Assert.False(result);
        mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingVideo_ReturnsVideoDto()
    {
        var video = new Video
        {
            Id = 1,
            Title = "Test Video",
            Duration = 120,
            Quality = 1080
        };

        mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(video);

        var result = await videoService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Video", result.Title);
        Assert.Equal(120, result.Duration);
        Assert.Equal(1080, result.Quality);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistentVideo_ReturnsNull()
    {
        mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Video?)null);

        var result = await videoService.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllVideos()
    {
        var videos = new List<Video>
        {
            new Video { Id = 1, Title = "Video 1", Duration = 120, Quality = 1080 },
            new Video { Id = 2, Title = "Video 2", Duration = 180, Quality = 720 }
        };

        mockRepository.Setup(r => r.GetWhereAsync(It.IsAny<Expression<Func<Video, bool>>>()))
            .ReturnsAsync(videos);

        var result = await videoService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, v => v.Title == "Video 1");
        Assert.Contains(result, v => v.Title == "Video 2");
    }

    [Fact]
    public async Task GetAllAsync_NoVideos_ReturnsEmptyCollection()
    {
        mockRepository.Setup(r => r.GetWhereAsync(It.IsAny<Expression<Func<Video, bool>>>()))
            .ReturnsAsync(new List<Video>());

        var result = await videoService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}