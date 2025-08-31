using EducationPortal.Data.Models;
using EducationPortal.Data.Repo.Repositories;
using EducationPortal.Logic.DTOs;
using EducationPortal.Logic.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace EducationPortal.Logic.Services;

public class CourseService : ICourseService
{
    private readonly UnitOfWorkRepository unitOfWorkRepository;
    
    public CourseService(UnitOfWorkRepository unitOfWorkRepository)
    {
        this.unitOfWorkRepository = unitOfWorkRepository;
    }
    
    public bool Insert(CourseDTO course)
    {
        unitOfWorkRepository.Courses.Insert(
            new Course() { Name = course.Name, Description = course.Description });
        unitOfWorkRepository.Save();
        return true;
    }

    public bool Update(CourseDTO course)
    {
        Course? c = unitOfWorkRepository.Courses.GetById(course.Id);
        if (c == null) return false;
        unitOfWorkRepository.Courses.Update(
            new Course() { Name = course.Name, Description = course.Description });
        unitOfWorkRepository.Save();
        return true;
    }

    public bool Delete(int id)
    {
        unitOfWorkRepository.Courses.Delete(id);
        unitOfWorkRepository.Save();
        return true;
    }

    public CourseDTO GetById(int id)
    {
        return new CourseDTO(unitOfWorkRepository.Courses.GetById(id));
    }

    public CourseDTO GetByName(string name)
    {
        return new CourseDTO(unitOfWorkRepository.Courses.GetByName(name));
    }

    public IEnumerable<CourseDTO> GetAll()
    {
        return unitOfWorkRepository.Courses.GetAll().Select(c => new CourseDTO(c)).ToList();
    }

    public bool AddSkillToCourse(int courseId, int skillId)
    {
        unitOfWorkRepository.Courses.AddSkillToCourse(courseId, skillId);
        unitOfWorkRepository.Save();
        return true;
    }

    public bool AddMaterialToCourse(int courseId, int materialId)
    {
        unitOfWorkRepository.Courses.AddMaterialToCourse(courseId, materialId);
        unitOfWorkRepository.Save();
        return true;
    }
    
    public bool InsertWithRelations(CourseDTO course, List<int> skillIds, List<int> materialIds)
    {
        using var transaction = unitOfWorkRepository.BeginTransaction();
        try
        {
            var newCourse = new Course { Name = course.Name, Description = course.Description };
            unitOfWorkRepository.Courses.Insert(newCourse);
            unitOfWorkRepository.Save();
            
            foreach (var skillId in skillIds)
            {
                if (!AddSkillToCourse(newCourse.Id, skillId))
                    throw new Exception($"Failed to add skill {skillId}");
            }
            
            foreach (var materialId in materialIds)
            {
                if (!AddMaterialToCourse(newCourse.Id, materialId))
                    throw new Exception($"Failed to add material {materialId}");
            }
            
            transaction.Commit();
            return true;
        }
        catch
        {
            transaction.Rollback();
            return false;
        }
    }
    // TODO: Add service methods signatures to this service
}