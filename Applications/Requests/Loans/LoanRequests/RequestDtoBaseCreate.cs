namespace Applications.Requests.Loans.LoanRequests;

public class RequestDtoBaseCreate
{
    public RequestDtoBaseCreate(int? branchId, string? customerId, string? loanRequestId, string? memo)
    {
        LoanRequestId = loanRequestId;
        BranchId = branchId;
        Memo = memo;
        CustomerId = customerId;
    }
    public string? CustomerId { get; set; }
    public string? LoanRequestId { get; set; }
    public int? BranchId { get; set; }
    public string? Memo { get; set; } = string.Empty;
}
