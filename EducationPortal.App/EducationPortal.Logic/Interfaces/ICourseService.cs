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
}