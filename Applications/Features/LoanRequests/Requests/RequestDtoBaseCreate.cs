namespace Applications.Features.LoanRequests.Requests;

public class RequestDtoBaseCreate
{
    public RequestDtoBaseCreate(int? branchId, string? customerId, string? loanRequestId, string? memo)
    {
        LoanRequestId = loanRequestId;
        BranchId = branchId;
        Memo = memo;
        Remark = memo;
        CustomerId = customerId;
    }
    public string? CustomerId { get; set; }
    public string? LoanRequestId { get; set; }
    public int? BranchId { get; set; }
    public string? Remark { get; set; } = string.Empty; 
    public string? Memo { get; set; } = string.Empty;
}
