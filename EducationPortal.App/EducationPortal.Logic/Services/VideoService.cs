using EducationPortal.Data.Models;
using EducationPortal.Data.Repo.RepositoryInterfaces;
using EducationPortal.Logic.DTOs;
using EducationPortal.Logic.Interfaces;

namespace EducationPortal.Logic.Services;

public class VideoService : IVideoService
{
    private readonly IUnitOfWork unitOfWork;
    
    public VideoService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }
    
    public async Task<bool> InsertAsync(VideoDTO video)
    {
        var videoEntity = new Video
        {
            Title = video.Title,
            Duration = video.Duration,
            Quality = video.Quality
        };
        await unitOfWork.Repository<Video, int>().InsertAsync(videoEntity);
        return await unitOfWork.SaveAsync();
    }

    public async Task<bool> UpdateAsync(VideoDTO video)
    {
        var existingVideo = await unitOfWork.Repository<Video, int>().GetByIdAsync(video.Id);
        if (existingVideo == null) return false;
        
        existingVideo.Title = video.Title;
        existingVideo.Duration = video.Duration;
        existingVideo.Quality = video.Quality;
        await unitOfWork.Repository<Video, int>().UpdateAsync(existingVideo);
        return await unitOfWork.SaveAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await unitOfWork.Repository<Video, int>().DeleteAsync(id);
        return await unitOfWork.SaveAsync();
    }

    public async Task<VideoDTO?> GetByIdAsync(int id)
    {
        var video = await unitOfWork.Repository<Video, int>().GetByIdAsync(id);
        return video != null ? new VideoDTO(video) : null;
    }

    public async Task<IEnumerable<VideoDTO>> GetAllAsync()
    {
        var videos = await unitOfWork.Repository<Video, int>()
            .GetWhereAsync(v => true);
        return videos.Select(v => new VideoDTO(v)).ToList();
    }
}