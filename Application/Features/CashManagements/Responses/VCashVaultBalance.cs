namespace Application.Features.CashManagements.Responses;

public sealed class VCashVaultBalance
{
    public int? CurrencyId { get; set; }
    public string? CurrencyName { get; set; }
    public string? CurrencyCode { get; set; }
    public int? BranchRefId { get; set; }
    public decimal? VaultBalance { get; set; }

}
