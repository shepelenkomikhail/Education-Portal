using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationPortal.Data.Models;

public class CourseSkill
{
    public int CourseId { get; set; }
    
    [ForeignKey(nameof(CourseId))]
    [InverseProperty(nameof(Course.CourseSkills))]
    public virtual Course Course { get; set; }
    
    public int SkillId { get; set; }
    
    [ForeignKey(nameof(SkillId))]
    [InverseProperty(nameof(Skill.CourseSkills))]
    public virtual Skill Skill { get; set; }
}
