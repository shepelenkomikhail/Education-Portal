using EducationPortal.Data.Models;
using EducationPortal.Data.Repo.Repositories;
using EducationPortal.Logic.DTOs;
using EducationPortal.Logic.Interfaces;

namespace EducationPortal.Logic.Services;

public class CourseService : ICourseService
{
    private readonly CourseRepository courseRepository;
    
    public CourseService(CourseRepository courseRepository)
    {
        this.courseRepository = courseRepository;
    }
    
    public bool Insert(CourseDTO course)
    {
        return courseRepository.Insert(
            new Course() { Name = course.Name, Description = course.Description });
    }

    public bool Update(CourseDTO course)
    {
        Course? c = courseRepository.GetById(course.Id);
        if (c == null) return false;
        courseRepository.Update(
            new Course() { Name = course.Name, Description = course.Description });
        return true;
    }

    public bool Delete(int id)
    {
        return courseRepository.Delete(id);
    }

    public CourseDTO GetById(int id)
    {
        return new CourseDTO(courseRepository.GetById(id));
    }

    public IEnumerable<CourseDTO> GetAll()
    {
        return courseRepository.GetAll().Select(c => new CourseDTO(c)).ToList();
    }
}