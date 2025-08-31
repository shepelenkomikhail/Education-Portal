using EducationPortal.Logic.DTOs;

namespace EducationPortal.Logic.Interfaces;

public interface ICourseService
{
    bool Insert(CourseDTO course);
    bool Update(CourseDTO course);
    bool Delete(int id);
    CourseDTO GetById(int id);
    CourseDTO GetByName(string name);
    IEnumerable<CourseDTO> GetAll();
    bool AddSkillToCourse(int courseId, int skillId);
    bool AddMaterialToCourse(int courseId, int materialId);
    bool InsertWithRelations(CourseDTO course, List<int> skillIds, List<int> materialIds);
    // TODO: Add service methods signatures to this service
}