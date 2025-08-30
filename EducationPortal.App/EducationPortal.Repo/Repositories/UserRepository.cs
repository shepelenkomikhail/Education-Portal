using EducationPortal.Data.Models;
using EducationPortal.Data.Repo.RepositoryInterfaces;

namespace EducationPortal.Data.Repo.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(PortalDbContext context) : base(context)
    {
    }
    // TODO: Add repository methods specific to this repository
}