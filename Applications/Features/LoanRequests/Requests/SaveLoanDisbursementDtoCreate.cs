namespace Applications.Features.LoanRequests.Requests;

public sealed class SaveLoanDisbursementDtoCreate : RequestDtoBaseCreate
{
    public SaveLoanDisbursementDtoCreate(int? branchId, string? customerId, string? loanRequestId, string? memo) : base(branchId, customerId, loanRequestId, memo)
    {

    }
}
