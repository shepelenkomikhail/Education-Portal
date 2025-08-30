using EducationPortal.Data.Models;
using EducationPortal.Data.Repo.RepositoryInterfaces;

namespace EducationPortal.Data.Repo.Repositories;

public class VideoRepository : BaseRepository<Video>, IVideoRepository
{
    public VideoRepository(PortalDbContext context) : base(context)
    {
    }
    // TODO: Add repository methods specific to this repository
}