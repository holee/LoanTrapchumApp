namespace Applications.Requests.Loans.LoanRequests;

public class RequestDtoBaseCreate
{
    public RequestDtoBaseCreate(int? branchId, Guid? customerId, Guid? loanRequestId, string? memo)
    {
        LoanRequestId = loanRequestId;
        BranchId = branchId;
        Memo = memo;
        CustomerId = customerId;
    }
    public Guid? CustomerId { get; set; }
    public Guid? LoanRequestId { get; set; }
    public int? BranchId { get; set; }
    public string? Memo { get; set; } = string.Empty;
}
