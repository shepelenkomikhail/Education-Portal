using System.ComponentModel.DataAnnotations;

namespace EducationPortal.Data.Models;

public class Video
{
    [Required]
    public int Duration { get; set; }
    [Required]
    public int Quality { get; set; }
}