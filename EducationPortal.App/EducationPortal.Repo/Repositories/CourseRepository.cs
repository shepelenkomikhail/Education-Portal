using EducationPortal.Data.Models;
using EducationPortal.Data.Repo.RepositoryInterfaces;

namespace EducationPortal.Data.Repo.Repositories;

public class CourseRepository : BaseRepository<Course>, ICourseRepository 
{
    public CourseRepository(PortalDbContext context) : base(context)
    {
    }
    // TODO: Add repository methods specific to this repository
}