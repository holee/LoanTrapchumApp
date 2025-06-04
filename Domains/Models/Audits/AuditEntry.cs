namespace Domains.Models.Audits;

public sealed class AuditEntry : IBaseEntity<Guid>   // BaseEntity has Id + timestamps
{
    public required string UserId { get; init; }   // Identity user GUID
    public required string TableName { get; init; }
    public required string ActionType { get; init; } // Insert | Update | Delete
    public required string KeyValues { get; init; }  // PKs in JSON
    public string? OldValues { get; init; }          // JSON‐serialized before state
    public string? NewValues { get; init; }          // JSON‐serialized after state
}
