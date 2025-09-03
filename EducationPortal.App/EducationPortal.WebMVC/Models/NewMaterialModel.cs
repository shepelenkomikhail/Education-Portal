using System.ComponentModel.DataAnnotations;

namespace EducationPortal.WebMVC.Models;

public class NewMaterialModel
{
    [StringLength(50, ErrorMessage = "Material title cannot exceed 50 characters")]
    public string Title { get; set; } = string.Empty;
    
    public string MaterialType { get; set; } = string.Empty;
    
    public string? Author { get; set; }
    public int? PageAmount { get; set; }
    public string? Formant { get; set; }
    public DateTime? PublicationDate { get; set; }
    
    public int? Duration { get; set; }
    public int? Quality { get; set; }
    
    public DateTime? Date { get; set; }
    public string? Resource { get; set; }
}
