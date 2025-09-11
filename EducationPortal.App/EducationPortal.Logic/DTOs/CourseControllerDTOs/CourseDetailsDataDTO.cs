namespace EducationPortal.Logic.DTOs;

public class CourseDetailsDataDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<SkillDTO> Skills { get; set; } = new();
    public List<MaterialDTO> Materials { get; set; } = new();
    public List<BookDTO> Books { get; set; } = new();
    public List<ArticleDTO> Articles { get; set; } = new();
    public List<VideoDTO> Videos { get; set; } = new();
    public bool IsUserEnrolled { get; set; } = false;
    public int CompletionPercentage { get; set; } = 0;
    public List<int> CompletedMaterialIds { get; set; } = new();
}
