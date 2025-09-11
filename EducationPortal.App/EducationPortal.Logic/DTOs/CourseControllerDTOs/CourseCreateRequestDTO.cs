namespace EducationPortal.Logic.DTOs;

public class CourseCreateRequestDTO
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int CreatedByUserId { get; set; }
    public List<int> SelectedSkillIds { get; set; } = new();
    public List<int> SelectedMaterialIds { get; set; } = new();
    public List<NewSkillRequestDTO> NewSkills { get; set; } = new();
    public List<NewMaterialRequestDTO> NewMaterials { get; set; } = new();
}
