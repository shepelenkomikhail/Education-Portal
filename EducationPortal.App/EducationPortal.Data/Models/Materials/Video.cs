using System.ComponentModel.DataAnnotations;

namespace EducationPortal.Data.Models;

public class Video : Material
{
    [Required]
    public int Duration { get; set; }
    
    [Required]
    public int Quality { get; set; }
}