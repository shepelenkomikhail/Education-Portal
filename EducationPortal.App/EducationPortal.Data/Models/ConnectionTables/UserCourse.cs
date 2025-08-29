using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationPortal.Data.Models;

public class UserCourse
{
    public int UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    [InverseProperty(nameof(User.UserCourses))]
    public virtual User User { get; set; }
    public int CourseId { get; set; }
    [ForeignKey(nameof(CourseId))]
    [InverseProperty(nameof(Course.UserCourses))]
    public virtual Course Course { get; set; }
    [Required]
    public int CompletionPercentage { get; set; }  
}