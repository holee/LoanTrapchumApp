namespace Applications.Requests.Loans;

public sealed class LoanChangeStatusDtoCreate 
{
    public LoanChangeStatusDtoCreate()
    {

    }

    public LoanChangeStatusDtoCreate(long loanRequestId, int branchId, int statusId)
    {
        LoanRequestId = loanRequestId;
        BranchId = branchId;
        StatusId = statusId;
    }
    public LoanChangeStatusDtoCreate(long loanRequestId, int branchId, int statusId, long aprovedUserId = 0)
    {
        LoanRequestId = loanRequestId;
        BranchId = branchId;
        StatusId = statusId;
        AprovedUserId = aprovedUserId;
    }
    public long LoanRequestId { get; set; }
    public int BranchId { get; set; }
    public int StatusId { get; set; } = 1;
    public long AprovedUserId { get; set; }
}

