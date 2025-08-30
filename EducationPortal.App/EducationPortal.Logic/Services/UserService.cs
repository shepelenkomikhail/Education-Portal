using EducationPortal.Data.Repo.Repositories;
using EducationPortal.Logic.Interfaces;

namespace EducationPortal.Logic.Services;

public class UserService: IUserService
{
    private readonly UnitOfWorkRepository unitOfWorkRepository;
    
    public UserService(UnitOfWorkRepository unitOfWorkRepository)
    {
        this.unitOfWorkRepository = unitOfWorkRepository;
    }
    // TODO: Add service methods specific to this service
}