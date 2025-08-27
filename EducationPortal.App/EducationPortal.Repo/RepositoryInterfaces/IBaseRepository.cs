namespace EducationPortal.Data.Repo.RepositoryInterfaces;

public interface IBaseRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> GetAll();
    TEntity? GetById(int id);
    bool Insert(TEntity entity);
    bool Update(TEntity entity);
    bool Delete(TEntity entity);
    bool Delete(int id);
}