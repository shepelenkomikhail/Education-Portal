using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationPortal.Data.Models;

public class Skill : BaseEntity<int>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;
    
    public virtual ICollection<UserSkill> UserSkills { get; set; } = new HashSet<UserSkill>();
    public virtual ICollection<Course> Courses { get; set; } = new HashSet<Course>();
}