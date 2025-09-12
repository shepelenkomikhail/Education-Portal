namespace EducationPortal.Logic.DTOs;

public class CourseEditDataDTO
{
    public CourseDTO Course { get; set; } = new();
    public List<SkillDTO> Skills { get; set; } = new();
    public List<MaterialDTO> Materials { get; set; } = new();
    public List<SkillDTO> CourseSkills { get; set; } = new();
    public List<MaterialDTO> CourseMaterials { get; set; } = new();
    public bool CanEdit { get; set; }
}
