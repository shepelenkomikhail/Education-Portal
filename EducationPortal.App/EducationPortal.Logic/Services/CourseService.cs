using EducationPortal.Data.Models;
using EducationPortal.Data.Repo.RepositoryInterfaces;
using EducationPortal.Logic.DTOs;
using EducationPortal.Logic.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace EducationPortal.Logic.Services;

public class CourseService : ICourseService
{
    private readonly IUnitOfWork unitOfWork;
    
    public CourseService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }
    
    public async Task<bool> InsertAsync(CourseDTO course)
    {
        var courseEntity = new Course() { Name = course.Name, Description = course.Description };
        await unitOfWork.Repository<Course, int>().InsertAsync(courseEntity);
        return await unitOfWork.SaveAsync();
    }

    public async Task<bool> UpdateAsync(CourseDTO course)
    {
        var existingCourse = await unitOfWork.Repository<Course, int>().GetByIdAsync(course.Id);
        if (existingCourse == null) return false;
        
        existingCourse.Name = course.Name;
        existingCourse.Description = course.Description;
        await unitOfWork.Repository<Course, int>().UpdateAsync(existingCourse);
        return await unitOfWork.SaveAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await unitOfWork.Repository<Course, int>().DeleteAsync(id);
        return await unitOfWork.SaveAsync();
    }

    public async Task<CourseDTO?> GetByIdAsync(int id)
    {
        var course = await unitOfWork.Repository<Course, int>().GetByIdAsync(id);
        return course != null ? new CourseDTO(course) : null;
    }

    public async Task<CourseDTO?> GetByNameAsync(string name)
    {
        var course = await unitOfWork.Repository<Course, int>().GetSingleOrDefaultAsync(c => c.Name == name);
        return course != null ? new CourseDTO(course) : null;
    }

    public async Task<IEnumerable<CourseDTO>> GetAllAsync()
    {
        var courses = await unitOfWork.Repository<Course, int>().GetWhereAsync(c => true);
        return courses.Select(c => new CourseDTO(c)).ToList();
    }
    
    public async Task<IEnumerable<SkillDTO>> GetSkillsForCourse(int courseId)
    {
        var course = await unitOfWork.Repository<Course, int>().GetByIdAsync(courseId);
        return course?.Skills.Select(s => new SkillDTO(s)) ?? new List<SkillDTO>();
    }
    
    public async Task<IEnumerable<MaterialDTO>> GetMaterialsForCourse(int courseId)
    {
        var course = await unitOfWork.Repository<Course, int>().GetByIdAsync(courseId);
        return course?.Materials.Select(m => new MaterialDTO(m)) ?? new List<MaterialDTO>();
    }

    public async Task<bool> AddSkillToCourseAsync(int courseId, int skillId)
    {
        var course = await unitOfWork.Repository<Course, int>().GetByIdAsync(courseId);
        var skill = await unitOfWork.Repository<Skill, int>().GetByIdAsync(skillId);
        
        if (course == null || skill == null) return false;

        course.Skills.Add(skill);
        await unitOfWork.Repository<Course, int>().UpdateAsync(course);
        return await unitOfWork.SaveAsync();
    }

    public async Task<bool> AddMaterialToCourseAsync(int courseId, int materialId)
    {
        var course = await unitOfWork.Repository<Course, int>().GetByIdAsync(courseId);
        var material = await unitOfWork.Repository<Material, int>().GetByIdAsync(materialId);
        
        if (course == null || material == null) return false;

        course.Materials.Add(material);
        await unitOfWork.Repository<Course, int>().UpdateAsync(course);
        return await unitOfWork.SaveAsync();
    }
    
    public async Task<bool> InsertWithRelationsAsync(CourseDTO course, List<int> skillIds, List<int> materialIds)
    {
        using var transaction = unitOfWork.BeginTransaction();
        try
        {
            var newCourse = new Course { Name = course.Name, Description = course.Description };
            await unitOfWork.Repository<Course, int>().InsertAsync(newCourse);
            await unitOfWork.SaveAsync();
            
            foreach (var skillId in skillIds)
            {
                if (!await AddSkillToCourseAsync(newCourse.Id, skillId))
                    throw new Exception($"Failed to add skill {skillId}");
            }
            
            foreach (var materialId in materialIds)
            {
                if (!await AddMaterialToCourseAsync(newCourse.Id, materialId))
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
}