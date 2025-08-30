using EducationPortal.Data.Models;
using EducationPortal.Data.Repo.Repositories;
using EducationPortal.Logic.DTOs;
using EducationPortal.Logic.Interfaces;

namespace EducationPortal.Logic.Services;

public class SkillService: ISkillService
{
    private readonly UnitOfWorkRepository unitOfWorkRepository;
    
    public SkillService(UnitOfWorkRepository unitOfWorkRepository)
    {
        this.unitOfWorkRepository = unitOfWorkRepository;
    }
    
    public bool Insert(SkillDTO skill)
    {
        return unitOfWorkRepository.Skills.Insert(
            new Skill() { Name = skill.Name });
    }

    public bool Update(SkillDTO skill)
    {
        Skill? c = unitOfWorkRepository.Skills.GetById(skill.Id);
        if (c == null) return false;
        unitOfWorkRepository.Skills.Update(
            new Skill() { Name = skill.Name });
        return true;
    }

    public bool Delete(int id)
    {
        return unitOfWorkRepository.Skills.Delete(id);
    }

    public SkillDTO GetById(int id)
    {
        return new SkillDTO(unitOfWorkRepository.Skills.GetById(id));
    }

    public IEnumerable<SkillDTO> GetAll()
    {
        return unitOfWorkRepository.Skills.GetAll().Select(c => new SkillDTO(c)).ToList();
    }
    // TODO: Add service methods specific to this service
}