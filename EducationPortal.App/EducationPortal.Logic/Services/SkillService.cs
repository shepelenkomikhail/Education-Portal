using EducationPortal.Data.Repo.Repositories;
using EducationPortal.Logic.Interfaces;

namespace EducationPortal.Logic.Services;

public class SkillService: ISkillService
{
    private readonly UnitOfWorkRepository unitOfWorkRepository;
    
    public SkillService(UnitOfWorkRepository unitOfWorkRepository)
    {
        this.unitOfWorkRepository = unitOfWorkRepository;
    }
    // TODO: Add service methods specific to this service
}