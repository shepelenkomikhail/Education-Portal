namespace EducationPortal.Logic.DTOs;

public class CourseIndexDataDTO
{
    public List<CourseWithPermissionsDTO> Courses { get; set; } = new();
    public bool IsAdmin { get; set; }
    public int? CurrentUserId { get; set; }
}

public class CourseWithPermissionsDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? CreatedByUserId { get; set; }
    public bool CanEdit { get; set; }
}
