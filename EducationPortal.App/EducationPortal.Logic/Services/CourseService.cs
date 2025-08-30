using EducationPortal.Data.Models;
using EducationPortal.Data.Repo.Repositories;
using EducationPortal.Logic.DTOs;
using EducationPortal.Logic.Interfaces;

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
        return unitOfWorkRepository.Courses.Insert(
            new Course() { Name = course.Name, Description = course.Description });
    }

    public bool Update(CourseDTO course)
    {
        Course? c = unitOfWorkRepository.Courses.GetById(course.Id);
        if (c == null) return false;
        unitOfWorkRepository.Courses.Update(
            new Course() { Name = course.Name, Description = course.Description });
        return true;
    }

    public bool Delete(int id)
    {
        return unitOfWorkRepository.Courses.Delete(id);
    }

    public CourseDTO GetById(int id)
    {
        return new CourseDTO(unitOfWorkRepository.Courses.GetById(id));
    }

    public IEnumerable<CourseDTO> GetAll()
    {
        return unitOfWorkRepository.Courses.GetAll().Select(c => new CourseDTO(c)).ToList();
    }
}