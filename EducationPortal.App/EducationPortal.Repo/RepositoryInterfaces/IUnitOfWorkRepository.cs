using Microsoft.EntityFrameworkCore.Storage;

namespace EducationPortal.Data.Repo.RepositoryInterfaces;

public interface IUnitOfWorkRepository : IDisposable
{
    public bool Save();
    IDbContextTransaction BeginTransaction();
}