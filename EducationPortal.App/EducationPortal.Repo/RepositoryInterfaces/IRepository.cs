using EducationPortal.Data.Models;
using System.Linq.Expressions;

namespace EducationPortal.Data.Repo.RepositoryInterfaces;

public interface IRepository<TEntity, TId> where TEntity : BaseEntity<TId>
{
    IQueryable<TEntity> GetAll();
    Task<TEntity?> GetByIdAsync(TId id);
    Task<TEntity?> GetSingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate);
    Task<bool> InsertAsync(TEntity entity);
    Task<bool> UpdateAsync(TEntity entity);
    Task<bool> DeleteAsync(TEntity entity);
    Task<bool> DeleteAsync(TId id);
}