using Domains.Models.Identity;

namespace Domains.Models.Audits;

public class AuditTrail
{
    public required Guid Id { get; set; }
    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; }
    public TrailType TrailType { get; set; }
    public DateTime DateUtc { get; set; }
    public required string EntityName { get; set; }
    public string? PrimaryKey { get; set; }
    public string? OldValues { get; set; } = string.Empty;
    public string? NewValues { get; set; } = string.Empty;
    public string? ChangedColumns { get; set; } = string.Empty;
}
