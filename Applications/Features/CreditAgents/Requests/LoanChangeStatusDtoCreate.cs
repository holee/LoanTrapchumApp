namespace Applications.Features.CreditAgents.Requests;

public sealed class LoanChangeStatusDtoCreate 
{
    public LoanChangeStatusDtoCreate()
    {

    }

    public LoanChangeStatusDtoCreate(string? loanRequestId, int? branchId, int? statusId)
    {
        LoanRequestId = loanRequestId;
        BranchId = branchId;
        StatusId = statusId;
    }
    public LoanChangeStatusDtoCreate(string? loanRequestId, int? branchId, int? statusId, string? aprovedUserId)
    {
        LoanRequestId = loanRequestId;
        BranchId = branchId;
        StatusId = statusId;
        AprovedUserId = aprovedUserId;
    }
    public string? LoanRequestId { get; set; }
    public int? BranchId { get; set; }
    public int? StatusId { get; set; } = 1;
    public string? AprovedUserId { get; set; }
}

