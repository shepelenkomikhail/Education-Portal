using EducationPortal.Logic.DTOs;

namespace EducationPortal.Logic.Interfaces;

public interface IMaterialService
{
    MaterialDTO GetById(int id);
    IEnumerable<MaterialDTO> GetAll();
    // TODO: Add service methods signatures to this service
}