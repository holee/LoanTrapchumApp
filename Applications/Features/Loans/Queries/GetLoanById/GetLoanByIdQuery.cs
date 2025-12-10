using Applications.Enumerations;
using Applications.Responses;
using ErrorOr;

namespace Applications.Features.Loans.Queries.GetLoanById;

public record GetLoanByIdQuery(string? LoanId, int? BranchId, ScheduleStatus Status, bool Include = false)
    : IRequest<ErrorOr<VLoanDisbursments>>; 
