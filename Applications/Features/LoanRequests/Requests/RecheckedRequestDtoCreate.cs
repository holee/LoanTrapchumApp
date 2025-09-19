using Applications.Enumerations;

namespace Applications.Features.LoanRequests.Requests;


public sealed class RecheckedRequestDtoCreate : RequestDtoBaseCreate
{
    public RecheckedRequestDtoCreate(int? branchId, string? customerId, string? loanRequestId, 
               DateTime? checkedDate,RequestStatus status, string? memo)
        : base(branchId, customerId, loanRequestId, memo)
    {

        RequestStatusId=(int)status;

        CheckedDate = checkedDate;
    }

    public int RequestStatusId { get; set; } = (int)RequestStatus.Rechecked;
    public DateTime? CheckedDate { get; set; } = DateTime.UtcNow;
}