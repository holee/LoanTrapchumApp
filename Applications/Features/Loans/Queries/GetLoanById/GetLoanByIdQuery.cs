using Applications.Enumerations;
using Applications.Features.Loans.Responses;
using Applications.Responses;
using Applications.Utils;

namespace Applications.Features.Loans.Queries.GetLoanById;

public record GetLoanByIdQuery(string? LoanId, int? BranchId, ScheduleStatus Status, bool Include = false)
    : IRequest<Result<VLoanDisbursments>>; 
