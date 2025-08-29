using System.ComponentModel.DataAnnotations;

namespace EducationPortal.Data.Models;

public class Article : Material
{
    [Required]
    public DateTime Date { get; set; }
    
    [Required]
    public string Resource { get; set; } = string.Empty;
}