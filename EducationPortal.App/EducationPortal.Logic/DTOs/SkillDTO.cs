using EducationPortal.Data.Models;

namespace EducationPortal.Logic.DTOs;

public class SkillDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public SkillDTO(){}

    internal SkillDTO(Skill? skill)
    {
        ArgumentNullException.ThrowIfNull(skill, nameof(skill));
        
        Id = skill.Id;
        Name = skill.Name;
    }

    internal Skill ToSkill()
    {
        return new Skill() { Id = this.Id, Name = this.Name };
    }
}