using EducationPortal.Data.Models;
using EducationPortal.Data.Repo.Repositories;
using EducationPortal.Logic.DTOs;
using EducationPortal.Logic.Interfaces;

namespace EducationPortal.Logic.Services;

public class MaterialService: IMaterialService
{
    private readonly UnitOfWorkRepository unitOfWorkRepository;
    
    public MaterialService(UnitOfWorkRepository unitOfWorkRepository)
    {
        this.unitOfWorkRepository = unitOfWorkRepository;
    }
    
    public bool Delete(int id)
    {
        unitOfWorkRepository.Materials.Delete(id);
        unitOfWorkRepository.Save();
        return true;
    }

    public MaterialDTO GetById(int id)
    {
        return new MaterialDTO(unitOfWorkRepository.Materials.GetById(id));
    }

    public IEnumerable<MaterialDTO> GetAll()
    {
        return unitOfWorkRepository.Materials.GetAll().Select(c => new MaterialDTO(c)).ToList();
    }
    // TODO: Add service methods specific to this service
}