using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationPortal.Data.Models;

public class Course : BaseEntity<int>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [StringLength(3000)]
    public string Description { get; set; } = string.Empty;
    
    public virtual ICollection<Material> Materials { get; set; } = new HashSet<Material>();
    public virtual ICollection<Skill> Skills { get; set; } = new HashSet<Skill>();
    public virtual ICollection<UserCourse> UserCourses { get; set; } = new HashSet<UserCourse>();
}