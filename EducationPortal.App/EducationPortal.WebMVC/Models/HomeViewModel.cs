using EducationPortal.Logic.DTOs;

namespace EducationPortal.WebMVC.Models;

public class HomeViewModel
{
    public bool IsAuthenticated { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<UserSkillViewModel> UserSkills { get; set; } = new List<UserSkillViewModel>();
}

public class UserSkillViewModel
{
    public string SkillName { get; set; } = string.Empty;
    public int SkillLevel { get; set; }
}
