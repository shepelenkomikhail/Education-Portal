namespace EducationPortal.Data.Models;

public interface IBaseEntity<TId>
{
    TId Id { get; set; }
}