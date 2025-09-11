namespace EducationPortal.Logic.DTOs;

public class UserCourseProgressDTO
{
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public string CourseDescription { get; set; } = string.Empty;
    public int CompletionPercentage { get; set; }
    public int TotalMaterials { get; set; }
    public int CompletedMaterials { get; set; }
}