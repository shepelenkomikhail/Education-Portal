using EducationPortal.Data.Models;

namespace EducationPortal.Data.Repo.RepositoryInterfaces;

public interface IRepositoryFactory
{
    IRepository<T, TId> GetRepository<T, TId>() where T : BaseEntity<TId>;
}