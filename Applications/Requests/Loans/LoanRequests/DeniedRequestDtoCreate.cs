namespace Applications.Requests.Loans.LoanRequests;

public class DeniedRequestDtoCreate : RequestDtoBase
{
    public DeniedRequestDtoCreate(int? branchId, long customerId, long? loanRequestId, DateTime? deniedDate, string? memo)
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
