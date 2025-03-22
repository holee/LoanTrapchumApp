namespace Applications.Requests.Loans.LoanRequests;

public class AproveLoanDtoCreate : RequestDtoBaseCreate
{
    public AproveLoanDtoCreate(int? branchId, Guid? customerId, 
        Guid? loanRequestId, 
        decimal? aprovedAmount,
        decimal? adminFeeAmount,
        DateTime? aprovedDate,
        string? memo)
    : base(branchId, customerId, loanRequestId, memo)
    {
        LoanRequestId = loanRequestId;
        AprovedDate = aprovedDate;
        Memo = memo;
        BranchId = branchId;
        CustomerId = customerId;
        AprovedAmount = aprovedAmount;
        AdminFeeAmount = adminFeeAmount;
    }
    public DateTime? AprovedDate { get; set; } = DateTime.UtcNow;
    public decimal? AprovedAmount { get; set; } = 0;
    public decimal? AdminFeeAmount { get; set; } = 0;
}
