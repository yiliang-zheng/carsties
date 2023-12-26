namespace Shared.Domain.Interface;

public interface IAuditableEntity
{
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}