using EducationPortal.Logic.DTOs;

namespace EducationPortal.Logic.Interfaces;

public interface ISkillService
{
    bool Insert(SkillDTO skill);
    bool Update(SkillDTO skill);
    bool Delete(int id);
    SkillDTO GetById(int id);
    IEnumerable<SkillDTO> GetAll(); 
    // TODO: Add service methods signatures to this service
}