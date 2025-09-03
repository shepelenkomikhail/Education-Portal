using System.ComponentModel.DataAnnotations;

namespace EducationPortal.WebMVC.Models;

public class NewSkillModel
{
    [StringLength(50, ErrorMessage = "Skill name cannot exceed 50 characters")]
    public string Name { get; set; } = string.Empty;
}
