namespace Applications.Features.LoanPayments.Dtos;

public sealed class PayOffResult
{
    public string? LoanId { get; set; } = string.Empty;
    public int? BranchId { get; set; } = 0;
    public decimal? Principle { get; set; } = 0;
    public decimal? Interest { get; set; } = 0;
    public decimal? CollectionFee { get; set; } = 0;
    public decimal? PenaltyAmount { get; set; } = 0;
    public decimal? Total { get; set; } = 0;
}
