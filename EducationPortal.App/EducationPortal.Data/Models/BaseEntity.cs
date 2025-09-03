namespace EducationPortal.Data.Models;

public abstract class BaseEntity<TId>
{
    public TId Id { get; set; }
}
