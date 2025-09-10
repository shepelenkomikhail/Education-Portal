using EducationPortal.Data.Models;
using EducationPortal.Data.Repo.RepositoryInterfaces;
using EducationPortal.Logic.DTOs;
using EducationPortal.Logic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EducationPortal.Logic.Services;

public class UserService: IUserService
{
    private readonly IUnitOfWork unitOfWork;
    
    public UserService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }
    
    public async Task<bool> InsertAsync(UserDTO user)
    {
        var userEntity = new User()
        {
            UserName = user.UserName,
            FirstName = user.FirstName,
            Surname = user.Surname,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };
        await unitOfWork.Repository<User, int>().InsertAsync(userEntity);
        return await unitOfWork.SaveAsync();
    }

    public async Task<bool> UpdateAsync(UserDTO user)
    {
        var existingUser = await unitOfWork.Repository<User, int>().GetByIdAsync(user.Id);
        if (existingUser == null) return false;

        existingUser.UserName = user.UserName;
        existingUser.FirstName = user.FirstName;
        existingUser.Surname = user.Surname;
        existingUser.Email = user.Email;
        existingUser.PhoneNumber = user.PhoneNumber;
        await unitOfWork.Repository<User, int>().UpdateAsync(existingUser);
        return await unitOfWork.SaveAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await unitOfWork.Repository<User, int>().DeleteAsync(id);
        return await unitOfWork.SaveAsync();
    }

    public async Task<UserDTO?> GetByIdAsync(int id)
    {
        var user = await unitOfWork.Repository<User, int>().GetByIdAsync(id);
        return user != null ? new UserDTO(user) : null;
    }

    public async Task<UserDTO?> GetByEmailAsync(string email)
    {
        var user = await unitOfWork.Repository<User, int>().GetSingleOrDefaultAsync(u => u.Email == email);
        return user != null ? new UserDTO(user) : null;
    }

    public async Task<IEnumerable<UserDTO>> GetAllAsync()
    {
        var users = await unitOfWork.Repository<User, int>().GetWhereAsync(u => true);
        return users.Select(u => new UserDTO(u)).ToList();
    }

    public async Task<UserDTO?> GetUserWithSkillsAsync(int userId)
    {
        var user = await unitOfWork.Repository<User, int>()
            .GetQueryable()
            .Include(u => u.UserSkills)
            .ThenInclude(us => us.Skill)
            .FirstOrDefaultAsync(u => u.Id == userId);

        return user != null ? new UserDTO(user) : null;
    }

    public async Task<UserDTO?> GetUserWithCoursesAsync(int userId)
    {
        var user = await unitOfWork.Repository<User, int>()
            .GetQueryable()
            .Include(u => u.UserCourses)
            .ThenInclude(uc => uc.Course)
            .FirstOrDefaultAsync(u => u.Id == userId);

        return user != null ? new UserDTO(user) : null;
    }

    public async Task<UserDTO?> GetUserWithSkillsAndCoursesAsync(int userId)
    {
        var user = await unitOfWork.Repository<User, int>()
            .GetQueryable()
            .Include(u => u.UserSkills)
            .ThenInclude(us => us.Skill)
            .Include(u => u.UserCourses)
            .ThenInclude(uc => uc.Course)
            .FirstOrDefaultAsync(u => u.Id == userId);

        return user != null ? new UserDTO(user) : null;
    }

}