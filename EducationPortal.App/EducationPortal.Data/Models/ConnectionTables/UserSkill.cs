using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationPortal.Data.Models;

public class UserSkill
{
    public int UserId { get; set; }
    [Required]
    [ForeignKey(nameof(UserId))]
    [InverseProperty(nameof(User.UserSkills))]
    public virtual User User { get; set; }  
    public int SkillId { get; set; }
    [Required]
    [ForeignKey(nameof(SkillId))]
    [InverseProperty(nameof(Skill.UserSkills))]
    public virtual Skill Skill { get; set; }
    [Required]
    public int SkillLevel { get; set; }
}