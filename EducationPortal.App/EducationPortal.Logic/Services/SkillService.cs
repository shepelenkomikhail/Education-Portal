using EducationPortal.Data.Models;
using EducationPortal.Data.Repo.RepositoryInterfaces;
using EducationPortal.Logic.DTOs;
using EducationPortal.Logic.Interfaces;

namespace EducationPortal.Logic.Services;

public class SkillService: ISkillService
{
    private readonly IUnitOfWork unitOfWork;
    
    public SkillService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }
    
    public async Task<bool> InsertAsync(SkillDTO skill)
    {
        var skillEntity = new Skill() { Name = skill.Name };
        await unitOfWork.Repository<Skill, int>().InsertAsync(skillEntity);
        return await unitOfWork.SaveAsync();
    }

    public async Task<bool> UpdateAsync(SkillDTO skill)
    {
        var existingSkill = await unitOfWork.Repository<Skill, int>().GetByIdAsync(skill.Id);
        if (existingSkill == null) return false;
        
        existingSkill.Name = skill.Name;
        await unitOfWork.Repository<Skill, int>().UpdateAsync(existingSkill);
        return await unitOfWork.SaveAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await unitOfWork.Repository<Skill, int>().DeleteAsync(id);
        return await unitOfWork.SaveAsync();
    }

    public async Task<SkillDTO?> GetByIdAsync(int id)
    {
        var skill = await unitOfWork.Repository<Skill, int>().GetByIdAsync(id);
        return skill != null ? new SkillDTO(skill) : null;
    }

    public async Task<IEnumerable<SkillDTO>> GetAllAsync()
    {
        var skills = await unitOfWork.Repository<Skill, int>().GetWhereAsync(s => true);
        return skills.Select(s => new SkillDTO(s)).ToList();
    }


}