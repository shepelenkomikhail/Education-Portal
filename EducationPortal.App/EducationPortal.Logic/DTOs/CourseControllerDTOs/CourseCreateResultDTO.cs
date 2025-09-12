namespace EducationPortal.Logic.DTOs;

public class CourseCreateResultDTO
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public List<string> ValidationErrors { get; set; } = new();
    public CourseCreateDataDTO? FallbackData { get; set; }
}
