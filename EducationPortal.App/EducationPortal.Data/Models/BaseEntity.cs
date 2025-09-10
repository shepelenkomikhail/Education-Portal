namespace EducationPortal.Data.Models;

public abstract class BaseEntity<TId> : IBaseEntity<TId>
{
    public TId Id { get; set; }
}