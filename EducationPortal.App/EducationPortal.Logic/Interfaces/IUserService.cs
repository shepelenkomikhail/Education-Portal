using EducationPortal.Logic.DTOs;

namespace EducationPortal.Logic.Interfaces;

public interface IUserService
{
    Task<bool> InsertAsync(UserDTO user);
    Task<bool> UpdateAsync(UserDTO user);
    Task<bool> DeleteAsync(int id);
    Task<UserDTO?> GetByIdAsync(int id);
    Task<UserDTO?> GetByEmailAsync(string email);
    Task<IEnumerable<UserDTO>> GetAllAsync();
    Task<UserDTO?> GetUserWithSkillsAsync(int userId);
    Task<UserDTO?> GetUserWithCoursesAsync(int userId);
    Task<UserDTO?> GetUserWithSkillsAndCoursesAsync(int userId);
    Task<bool> EnrollUserInCourseAsync(int userId, int courseId);
    Task<bool> IsUserEnrolledInCourseAsync(int userId, int courseId);
    Task<int> GetCourseProgressAsync(int userId, int courseId);
    Task<bool> UpdateCourseProgressAsync(int userId, int courseId, int completionPercentage);
    Task<bool> MarkMaterialAsCompletedAsync(int userId, int courseId, int materialId);
    Task<bool> IsMaterialCompletedAsync(int userId, int materialId);
    Task<UserDashboardDTO> GetUserDashboardDataAsync(int userId);
    Task<bool> AssignCourseSkillsToUserAsync(int userId, int courseId);
} 