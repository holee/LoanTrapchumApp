namespace Domains.Models.Audits;

public abstract class IBaseEntity<TId> 
{
    public TId Id { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
