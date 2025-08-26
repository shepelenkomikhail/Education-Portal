using System.ComponentModel.DataAnnotations;

namespace EducationPortal.Data.Models;

public class Book : Material
{
    [Required]
    public string Author { get; set; } = string.Empty;
    [Required]
    public int PageAmount { get; set; }
    [Required]
    public string Formant { get; set; } = string.Empty;
    [Required]
    public DateTime PublicationDate { get; set; }
}