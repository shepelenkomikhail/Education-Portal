namespace EducationPortal.Logic.DTOs;

public class CreatedCourseDTO
{
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public string CourseDescription { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int EnrolledStudents { get; set; }
    public int TotalMaterials { get; set; }
}