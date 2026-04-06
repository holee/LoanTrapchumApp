namespace Applications.Features.LoanRequests.Requests;

public class AproveLoanDtoCreate : RequestDtoBaseCreate
{
    public AproveLoanDtoCreate()
        : base(0, string.Empty, string.Empty, string.Empty)
    {

    }
    public AproveLoanDtoCreate(int? branchId, string? customerId,
        string? loanRequestId,
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
