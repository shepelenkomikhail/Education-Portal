using EducationPortal.Logic.DTOs;

namespace EducationPortal.Logic.Interfaces;

public interface ICourseService
{
    bool Insert(CourseDTO course);
    bool Update(CourseDTO course);
    bool Delete(int id);
    CourseDTO GetById(int id);
    IEnumerable<CourseDTO> GetAll();
}