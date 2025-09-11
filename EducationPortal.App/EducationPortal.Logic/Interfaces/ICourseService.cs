using EducationPortal.Logic.DTOs;

namespace EducationPortal.Logic.Interfaces;

public interface ICourseService
{
    Task<bool> InsertAsync(CourseDTO course);
    Task<bool> UpdateAsync(CourseDTO course);
    Task<bool> DeleteAsync(int id);
    Task<CourseDTO?> GetByIdAsync(int id);
    Task<CourseDTO?> GetByNameAsync(string name);
    Task<IEnumerable<CourseDTO>> GetAllAsync();
    Task<bool> AddSkillToCourseAsync(int courseId, int skillId);
    Task<bool> AddMaterialToCourseAsync(int courseId, int materialId);
    Task<IEnumerable<MaterialDTO>> GetMaterialsForCourse(int courseId);
    Task<IEnumerable<SkillDTO>> GetSkillsForCourse(int courseId);
    Task<bool> InsertWithRelationsAsync(CourseDTO course, List<int> skillIds, List<int> materialIds);
    Task<bool> UpdateWithRelationsAsync(CourseDTO course, List<int> skillIds, List<int> materialIds);
    Task<IEnumerable<CourseDTO>> GetCoursesByCreatorAsync(int userId);
    Task<CourseIndexDataDTO> GetCoursesWithPermissionsAsync(int? currentUserId, bool isAdmin);
    Task<CourseDetailsDataDTO> GetCourseDetailsWithUserDataAsync(int courseId, int? userId);
    Task<CourseCreateDataDTO> GetCourseCreateDataAsync();
    Task<CourseEditDataDTO> GetCourseEditDataAsync(int courseId, int? userId, bool isAdmin);
    Task<CourseCreateResultDTO> CreateCourseWithNewItemsAsync(CourseCreateRequestDTO request);
    Task<CourseEditResultDTO> UpdateCourseWithNewItemsAsync(CourseEditRequestDTO request);
    Task<MaterialDTO?> GetMaterialDetailsAsync(int materialId, string materialType);
}