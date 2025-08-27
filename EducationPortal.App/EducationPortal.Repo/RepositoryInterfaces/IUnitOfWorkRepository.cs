namespace EducationPortal.Data.Repo.RepositoryInterfaces;

public interface IUnitOfWorkRepository : IDisposable
{
    public bool Save();
}