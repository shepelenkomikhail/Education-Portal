using EducationPortal.Data.Models;
using EducationPortal.Data.Repo.RepositoryInterfaces;

namespace EducationPortal.Data.Repo.Repositories;

public class BookRepository : BaseRepository<Book>, IBookRepository
{
    public BookRepository(PortalDbContext context) : base(context)
    {
    }
    // TODO: Add repository methods specific to this repository
}