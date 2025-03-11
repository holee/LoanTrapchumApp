namespace Applications.Requests.Loans.LoanRequests;

public class RequestDtoBase
{
    public RequestDtoBase(int? branchId, long? customerId, long? loanRequestId, string? memo)
    {
        LoanRequestId = loanRequestId;
        BranchId = branchId;
        Memo = memo;
        CustomerId = customerId;
    }
    public long? CustomerId { get; set; }
    public long? LoanRequestId { get; set; }
    public int? BranchId { get; set; }
    public string? Memo { get; set; } = string.Empty;
}
