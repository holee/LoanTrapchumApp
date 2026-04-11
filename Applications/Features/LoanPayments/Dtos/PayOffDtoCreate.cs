namespace Applications.Features.LoanPayments.Dtos;

public sealed class PayOffDtoCreate
{
    public string? LoanId { get; set; } = string.Empty;
    public string? CustomerId { get; set; } = string.Empty;
    public int BranchId { get; set; }
}
