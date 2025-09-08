using EducationPortal.Logic.DTOs;

namespace EducationPortal.Logic.Interfaces;

public interface IUserService
{
    Task<bool> InsertAsync(UserDTO user);
    Task<bool> UpdateAsync(UserDTO user);
    Task<bool> DeleteAsync(int id);
    Task<UserDTO?> GetByIdAsync(int id);
    Task<UserDTO?> GetByEmailAsync(string email);
    Task<IEnumerable<UserDTO>> GetAllAsync();
}