using EducationPortal.Data.Models;

namespace EducationPortal.Logic.DTOs;

public class CourseDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public CourseDTO(){}

    internal CourseDTO(Course? course)
    {
        ArgumentNullException.ThrowIfNull(course, nameof(course));
        
        Id = course.Id;
        Name = course.Name;
        Description = course.Description;
    }

    internal Course ToCourse()
    {
        return new Course() { Id = this.Id, Name = this.Name, Description = this.Description };
    }
}