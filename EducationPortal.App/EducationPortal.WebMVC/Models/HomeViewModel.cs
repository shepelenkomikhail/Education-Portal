namespace EducationPortal.WebMVC.Models;

public class HomeViewModel
{
    public bool IsAuthenticated { get; set; }
    public bool IsAdmin { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<UserSkillViewModel> UserSkills { get; set; } = new List<UserSkillViewModel>();
    public List<UserCourseProgressViewModel> EnrolledCourses { get; set; } = new List<UserCourseProgressViewModel>();
    public List<UserCourseProgressViewModel> InProgressCourses { get; set; } = new List<UserCourseProgressViewModel>();
    public List<UserCourseProgressViewModel> CompletedCourses { get; set; } = new List<UserCourseProgressViewModel>();
    public List<CreatedCourseViewModel> CreatedCourses { get; set; } = new List<CreatedCourseViewModel>();
}