using EducationPortal.Logic.DTOs;

namespace EducationPortal.Logic.Interfaces;

public interface ISkillService
{
    Task<bool> InsertAsync(SkillDTO skill);
    Task<bool> UpdateAsync(SkillDTO skill);
    Task<bool> DeleteAsync(int id);
    Task<SkillDTO?> GetByIdAsync(int id);
    Task<IEnumerable<SkillDTO>> GetAllAsync();
}