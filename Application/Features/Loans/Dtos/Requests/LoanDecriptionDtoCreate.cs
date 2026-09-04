namespace Application.Features.Loans.Dtos.Requests;

public sealed class LoanDecriptionDtoCreate
{
    public string? LoanRequestRefId { get; set; }
    public string? Memo { get; set; } = string.Empty;
}
