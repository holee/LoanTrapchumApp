namespace Applications.Responses.Audits;

public sealed class AuditVm
{
    public Guid Id { get; init; }
    public string UserId { get; init; } = default!;
    public string? FullName { get; set; } = string.Empty;
    public string TableName { get; init; } = default!;
    public string ActionType { get; init; } = default!;
    public string KeyValues { get; init; } = default!;
    public string? OldValues { get; init; }
    public string? NewValues { get; init; }
    public DateTimeOffset? CreatedAt { get; init; }
}
