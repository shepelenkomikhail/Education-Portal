using EducationPortal.Data.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace EducationPortal.Data.Repo.RepositoryInterfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<T, TId> Repository<T, TId>() where T : class , IBaseEntity<TId>;
    Task<bool> SaveAsync();
    IDbContextTransaction BeginTransaction();
}