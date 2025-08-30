using EducationPortal.Data.Repo.Repositories;
using EducationPortal.Logic.Interfaces;

namespace EducationPortal.Logic.Services;

public class VideoService : IVideoService
{
    private readonly UnitOfWorkRepository unitOfWorkRepository;
    
    public VideoService(UnitOfWorkRepository unitOfWorkRepository)
    {
        this.unitOfWorkRepository = unitOfWorkRepository;
    }
    // TODO: Add service methods specific to this service
}