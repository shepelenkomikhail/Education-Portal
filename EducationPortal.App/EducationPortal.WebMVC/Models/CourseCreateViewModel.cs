using WebMVC.Models;

namespace EducationPortal.WebMVC.Models;

public class CourseCreateViewModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<SkillModel> Skills { get; set; } = new();
    public List<MaterialModel> Materials { get; set; } = new();
    public List<int> SelectedSkillIds { get; set; } = new();
    public List<int> SelectedMaterialIds { get; set; } = new();
}