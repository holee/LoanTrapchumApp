namespace Applications.Requests.Loans.LoanRequests;

public class DeniedRequestDtoCreate : RequestDtoBaseCreate
{
    public DeniedRequestDtoCreate(int? branchId, Guid? customerId, Guid? loanRequestId, DateTime? deniedDate, string? memo)
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
