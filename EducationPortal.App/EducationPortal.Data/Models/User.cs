using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace EducationPortal.Data.Models;

public class User : IdentityUser<int>, IBaseEntity<int>
{
    [Required]
    [StringLength(50)]
    public string Surname { get; set; } = string.Empty;
    
    public virtual ICollection<UserSkill> UserSkills { get; set; } = new HashSet<UserSkill>();
    public virtual ICollection<UserCourse> UserCourses { get; set; } = new HashSet<UserCourse>();
}