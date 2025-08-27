using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace EducationPortal.Data.Models;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50)]
    public string Surname { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50)]
    public string Password { get; set; } = string.Empty;
    
    public string? Phone { get; set; } = string.Empty;
    public virtual ICollection<UserSkill> UserSkills { get; set; } = new HashSet<UserSkill>();
    public virtual ICollection<UserCourse> UserCourses { get; set; } = new HashSet<UserCourse>();
}