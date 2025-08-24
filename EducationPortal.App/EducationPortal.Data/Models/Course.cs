using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationPortal.Data.Models;

public class Course
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
    public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
}