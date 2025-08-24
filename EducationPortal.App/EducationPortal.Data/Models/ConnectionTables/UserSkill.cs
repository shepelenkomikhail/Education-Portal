using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationPortal.Data.Models;

public class UserSkill
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int UserId { get; set; }
    [Required]
    [ForeignKey(nameof(UserId))]
    [InverseProperty(nameof(User.Skills))]
    public virtual User User { get; set; }  
    public int SkillId { get; set; }
    [Required]
    [ForeignKey(nameof(SkillId))]
    [InverseProperty(nameof(Skill.Users))]
    public virtual Skill Skill { get; set; }
    [Required]
    public int SkillLevel { get; set; }
}