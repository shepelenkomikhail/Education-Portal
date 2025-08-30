using EducationPortal.Data.Models;
using EducationPortal.Data.Repo.RepositoryInterfaces;

namespace EducationPortal.Data.Repo.Repositories;

public class SkillRepository : BaseRepository<Skill>, ISkillRepository
{
    public SkillRepository(PortalDbContext context) : base(context)
    {
    }
    // TODO: Add repository methods specific to this repository
}