using EducationPortal.Data.Repo.Repositories;
using EducationPortal.Logic.Interfaces;

namespace EducationPortal.Logic.Services;

public class BookService: IBookService
{
    private readonly UnitOfWorkRepository unitOfWorkRepository;
    
    public BookService(UnitOfWorkRepository unitOfWorkRepository)
    {
        this.unitOfWorkRepository = unitOfWorkRepository;
    }
    // TODO: Add service methods specific to this service
}