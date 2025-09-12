namespace EducationPortal.Logic.DTOs;

public class CourseEditRequestDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int UserId { get; set; }
    public bool IsAdmin { get; set; }
    public List<int> SelectedSkillIds { get; set; } = new();
    public List<int> SelectedMaterialIds { get; set; } = new();
    public List<NewSkillRequestDTO> NewSkills { get; set; } = new();
    public List<NewMaterialRequestDTO> NewMaterials { get; set; } = new();
}