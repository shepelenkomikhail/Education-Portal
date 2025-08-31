using EducationPortal.Data.Models;
using EducationPortal.Data.Repo.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace EducationPortal.Data.Repo.Repositories;

public class CourseRepository : BaseRepository<Course>, ICourseRepository 
{
    public CourseRepository(PortalDbContext context) : base(context)
    {
    }
    
    public Course? GetByName(string name)
    {
        return context.Courses.FirstOrDefault(c => c.Name == name);
    }

    public bool AddSkillToCourse(int courseId, int skillId)
    {
        var course = context.Courses.Include(c => c.Skills)
            .FirstOrDefault(c => c.Id == courseId);
        var skill = context.Skills.Find(skillId);

        if (course == null || skill == null)
            return false;

        course.Skills.Add(skill);
        return true;
    }

    public bool AddMaterialToCourse(int courseId, int materialId)
    {
        var course = context.Courses.Include(c => c.Materials)
            .FirstOrDefault(c => c.Id == courseId);
        var material = context.Materials.Find(materialId);

        if (course == null || material == null)
            return false;

        course.Materials.Add(material);
        return true;
    }
    // TODO: Add repository methods specific to this repository
}