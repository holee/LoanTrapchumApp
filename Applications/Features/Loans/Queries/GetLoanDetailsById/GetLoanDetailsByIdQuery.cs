using Applications.Enumerations;
using ErrorOr;

namespace Applications.Features.Loans.Queries.GetLoanDetailsById;

public record GetLoanDetailsByIdQuery(string? LoanId, int? BranchId, ScheduleStatus Status = ScheduleStatus.All, bool Include = false)
    : IRequest<ErrorOr<VLoanDisbursments>>; 
