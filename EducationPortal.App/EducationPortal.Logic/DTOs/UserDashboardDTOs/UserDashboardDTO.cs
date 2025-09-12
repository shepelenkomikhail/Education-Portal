namespace EducationPortal.Logic.DTOs;

public class UserDashboardDTO
{
    public UserInfoDTO UserInfo { get; set; } = new();
    public List<UserSkillDTO> UserSkills { get; set; } = new();
    public List<UserCourseProgressDTO> EnrolledCourses { get; set; } = new();
    public List<UserCourseProgressDTO> InProgressCourses { get; set; } = new();
    public List<UserCourseProgressDTO> CompletedCourses { get; set; } = new();
    public List<CreatedCourseDTO> CreatedCourses { get; set; } = new();
}