namespace EducationPortal.Logic.DTOs;

public class CourseIndexDataDTO
{
    public List<CourseWithPermissionsDTO> Courses { get; set; } = new();
    public bool IsAdmin { get; set; }
    public int? CurrentUserId { get; set; }
}
