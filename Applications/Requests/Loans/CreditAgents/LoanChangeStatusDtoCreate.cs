namespace Applications.Requests.Loans.CreditAgents;

public sealed class LoanChangeStatusDtoCreate 
{
    public LoanChangeStatusDtoCreate()
    {

    }

    public LoanChangeStatusDtoCreate(Guid? loanRequestId, int? branchId, int? statusId)
    {
        LoanRequestId = loanRequestId;
        BranchId = branchId;
        StatusId = statusId;
    }
    public LoanChangeStatusDtoCreate(Guid? loanRequestId, int? branchId, int? statusId, Guid? aprovedUserId)
    {
        LoanRequestId = loanRequestId;
        BranchId = branchId;
        StatusId = statusId;
        AprovedUserId = aprovedUserId;
    }
    public Guid? LoanRequestId { get; set; }
    public int? BranchId { get; set; }
    public int? StatusId { get; set; } = 1;
    public Guid? AprovedUserId { get; set; }
}

