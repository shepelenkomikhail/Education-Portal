using EducationPortal.Data.Models;

namespace EducationPortal.Data.Repo.RepositoryInterfaces;

public interface ICourseRepository: IBaseRepository<Course>
{
    bool AddSkillToCourse(int courseId, int skillId);
    bool AddMaterialToCourse(int courseId, int materialId);
    // TODO: Add repository signatures specific to this repository
}