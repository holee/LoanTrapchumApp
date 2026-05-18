namespace Applications.Features.Loans.Dtos;

public sealed class WidthrawCollateralDtoCreate
{
    public int? BranchId { get; set; }
    public long? CollateralLineId { get; set; } = default(long?); 
    public string? LoanId { get; set; } = string.Empty;
}
