using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationPortal.Data.Models;

public abstract class Material
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Title { get; set; } = string.Empty;
    
    public virtual ICollection<Course> Courses { get; set; } = new HashSet<Course>();

    public virtual ICollection<CourseMaterial> CourseMaterials { get; set; } = new HashSet<CourseMaterial>();
}