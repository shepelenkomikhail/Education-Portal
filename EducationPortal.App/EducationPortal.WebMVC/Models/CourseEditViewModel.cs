using System.ComponentModel.DataAnnotations;
using WebMVC.Models;

namespace EducationPortal.WebMVC.Models;

public class CourseEditViewModel
{
    [Required(ErrorMessage = "Course name is required")]
    [StringLength(50, ErrorMessage = "Course name cannot exceed 50 characters")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Course description is required")]
    [StringLength(3000, ErrorMessage = "Course description cannot exceed 3000 characters")]
    public string Description { get; set; } = string.Empty;
    
    public List<SkillModel> Skills { get; set; } = new();
    public List<MaterialModel> Materials { get; set; } = new();
    
    [Required(ErrorMessage = "Please select at least one skill")]
    public List<int> SelectedSkillIds { get; set; } = new();
    
    [Required(ErrorMessage = "Please select at least one material")]
    public List<int> SelectedMaterialIds { get; set; } = new();
}