using EducationPortal.Logic.DTOs;

namespace EducationPortal.Logic.Interfaces;

public interface IBookService
{
    Task<bool> InsertAsync(BookDTO book);
    Task<bool> UpdateAsync(BookDTO book);
    Task<bool> DeleteAsync(int id);
    Task<BookDTO?> GetByIdAsync(int id);
    Task<IEnumerable<BookDTO>> GetAllAsync();
    Task<IEnumerable<BookDTO>> GetByAuthorAsync(string author);
}