using EducationPortal.Data.Models;

namespace EducationPortal.Logic.DTOs;

public class UserDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Phone { get; set; } = string.Empty;
    
    public UserDTO(){}

    internal UserDTO(User? user)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        
        Id = user.Id;
        Name = user.Name;
        Surname = user.Surname;
        Email = user.Email;
        Password = user.Password;
        Phone = user.Phone;
    }

    internal User ToUser()
    {
        return new User() 
        { 
            Id = this.Id, 
            Name = this.Name, 
            Surname = this.Surname, 
            Email = this.Email, 
            Password = this.Password, 
            Phone = this.Phone 
        };
    }
}


