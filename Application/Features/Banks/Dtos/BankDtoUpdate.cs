namespace Application.Features.Banks.Dtos;

/// <summary>
/// DTO for updating an existing Bank
/// </summary>
public sealed record BankDtoUpdate
{
    public int BankId { get; init; }
    public string BankCode { get; init; } = null!;
    public string BankName { get; init; } = null!;
    public string? AltName { get; init; }
    public string? AltPhone { get; init; }
    public string? Email { get; init; }
    public string? WebSite { get; init; }
    public bool IsClosed { get; init; }
    public bool IsHead { get; init; }
    public string? Country { get; init; }
    public string? ProvinceName { get; init; }
    public string? DistrictName { get; init; }
    public string? CommuneName { get; init; }
    public string? VillageName { get; init; }
    public string? Remark { get; init; }
}
