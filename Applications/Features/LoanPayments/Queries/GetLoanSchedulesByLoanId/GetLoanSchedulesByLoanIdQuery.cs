using Applications.Enumerations;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Schedules;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.LoanPayments.Queries.GetLoanSchedulesByLoanIdAsPaging;

public record GetLoanSchedulesByLoanIdQuery(string? LoanId, int? BranchId, ScheduleStatus Status = ScheduleStatus.All, QueryParam? Param = null)
    : IRequest<ErrorOr<IEnumerable<VSchedule>>>;
