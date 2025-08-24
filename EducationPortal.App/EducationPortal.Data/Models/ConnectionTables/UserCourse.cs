using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationPortal.Data.Models;

public class UserCourse
{
    [Key] public int Id { get; set; }
    public int UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    [InverseProperty(nameof(User.Courses))]
    public User User { get; set; }
    public int CourseId { get; set; }
    [ForeignKey(nameof(CourseId))]
    [InverseProperty(nameof(Course.Users))]
    public Course Course { get; set; }
    [Required]
    public int CompletionPercentage { get; set; }  
}