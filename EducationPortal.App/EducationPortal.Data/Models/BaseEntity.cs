namespace EducationPortal.Data.Models;

public abstract class BaseEntity<TId> where TId : struct
{
    public TId Id { get; set; }
}
