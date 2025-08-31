using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationPortal.Data.Models;

public class CourseMaterial
{
    public int CourseId { get; set; }
    
    [ForeignKey(nameof(CourseId))]
    [InverseProperty(nameof(Course.CourseMaterials))]
    public virtual Course Course { get; set; }
    
    public int MaterialId { get; set; }
    
    [ForeignKey(nameof(MaterialId))]
    [InverseProperty(nameof(Material.CourseMaterials))]
    public virtual Material Material { get; set; }
}
