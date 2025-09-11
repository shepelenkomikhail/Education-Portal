namespace EducationPortal.Logic.DTOs;

public class CourseEditResultDTO
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public List<string> ValidationErrors { get; set; } = new();
    public CourseEditDataDTO? FallbackData { get; set; }
}
