using EducationPortal.Data.Repo.Repositories;
using EducationPortal.Logic.Interfaces;

namespace EducationPortal.Logic.Services;

public class MaterialService: IMaterialService
{
    private readonly UnitOfWorkRepository unitOfWorkRepository;
    
    public MaterialService(UnitOfWorkRepository unitOfWorkRepository)
    {
        this.unitOfWorkRepository = unitOfWorkRepository;
    }
    // TODO: Add service methods specific to this service
}