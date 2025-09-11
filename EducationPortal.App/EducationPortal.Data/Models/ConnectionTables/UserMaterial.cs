using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationPortal.Data.Models;

public class UserMaterial
{
    public int UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    [InverseProperty(nameof(User.UserMaterials))]
    public virtual User User { get; set; }
    
    public int MaterialId { get; set; }
    [ForeignKey(nameof(MaterialId))]
    [InverseProperty(nameof(Material.UserMaterials))]
    public virtual Material Material { get; set; }
    
    [Required]
    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;
    
    [Required]
    public bool IsCompleted { get; set; } = true;
}
