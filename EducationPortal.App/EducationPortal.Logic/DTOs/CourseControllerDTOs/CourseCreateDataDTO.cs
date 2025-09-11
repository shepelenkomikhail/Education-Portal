namespace EducationPortal.Logic.DTOs;

public class CourseCreateDataDTO
{
    public List<SkillDTO> Skills { get; set; } = new();
    public List<MaterialDTO> Materials { get; set; } = new();
}
