using EducationPortal.Data.Models;

namespace EducationPortal.Logic.DTOs;

public class UserDTO
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; } = string.Empty;
    
    public UserDTO(){}

    internal UserDTO(User? user)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        Id = user.Id;
        UserName = user.UserName ?? string.Empty;
        FirstName = user.FirstName;
        Surname = user.Surname;
        Email = user.Email ?? string.Empty;
        PhoneNumber = user.PhoneNumber;
    }

    internal User ToUser()
    {
        return new User()
        {
            Id = this.Id,
            UserName = this.UserName,
            FirstName = this.FirstName,
            Surname = this.Surname,
            Email = this.Email,
            PhoneNumber = this.PhoneNumber
        };
    }
}


