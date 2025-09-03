using EducationPortal.Logic.DTOs;

namespace EducationPortal.Logic.Interfaces;

public interface IMaterialService
{
    Task<bool> InsertAsync(MaterialDTO material);
    Task<bool> UpdateAsync(MaterialDTO material);
    Task<bool> DeleteAsync(int id);
    Task<MaterialDTO?> GetByIdAsync(int id);
    Task<IEnumerable<MaterialDTO>> GetAllAsync();
    Task<IEnumerable<MaterialDTO>> GetByTypeAsync(string materialType);
}