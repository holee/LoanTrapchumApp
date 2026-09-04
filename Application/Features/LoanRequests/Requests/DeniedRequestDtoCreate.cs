namespace Application.Features.LoanRequests.Requests;

public class DeniedRequestDtoCreate : RequestDtoBaseCreate
{
    public DeniedRequestDtoCreate(int? branchId, string? customerId, string? loanRequestId, DateTime? deniedDate, string? memo)
        : base(branchId, customerId, loanRequestId, memo)
    {
        LoanRequestId = loanRequestId;
        BranchId = branchId;
        DeniedDate = deniedDate;
        Memo = memo;
        CustomerId = customerId;
    }
    public DateTime? DeniedDate { get; set; } = DateTime.UtcNow;
}
