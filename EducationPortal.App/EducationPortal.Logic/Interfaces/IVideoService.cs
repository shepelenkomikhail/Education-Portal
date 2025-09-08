using EducationPortal.Logic.DTOs;

namespace EducationPortal.Logic.Interfaces;

public interface IVideoService
{
    Task<bool> InsertAsync(VideoDTO video);
    Task<bool> UpdateAsync(VideoDTO video);
    Task<bool> DeleteAsync(int id);
    Task<VideoDTO?> GetByIdAsync(int id);
    Task<IEnumerable<VideoDTO>> GetAllAsync();
}