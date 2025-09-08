using EducationPortal.Data.Models;

namespace EducationPortal.Logic.DTOs;

public class MaterialDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    
    public MaterialDTO(){}

    internal MaterialDTO(Material? material)
    {
        ArgumentNullException.ThrowIfNull(material, nameof(material));
        
        Id = material.Id;
        Title = material.Title;
    }
}