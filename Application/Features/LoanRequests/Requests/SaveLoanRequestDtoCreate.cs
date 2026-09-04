using Application.Enumerations;

namespace Application.Features.LoanRequests.Requests;

public sealed class SaveLoanRequestDtoCreate : RequestDtoBaseCreate
{
    public SaveLoanRequestDtoCreate(int? branchId, string? customerId, string? loanRequestId, string? memo) : base(branchId, customerId, loanRequestId, memo)
    {

    }

    public SaveLoanRequestDtoCreate(int? branchId, string? customerId, string? loanRequestId, string? memo, RequestStatus requestStatus) : base(branchId, customerId, loanRequestId, memo)
    {
        RequestStatus = requestStatus;
    }
    public RequestStatus RequestStatus { get; set; } = RequestStatus.New;
}
