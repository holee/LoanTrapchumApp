using Applications.Enumerations;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Schedules;
using Applications.Utils;

namespace Applications.Features.LoanPayments.Queries.GetLoanSchedulesByLoanIdAsPaging;

public record GetLoanSchedulesByLoanIdAsPagingQuery(string? LoanId, int? BranchId, ScheduleStatus Status = ScheduleStatus.All, QueryParam? Param = null)
    : IRequest<Pager<VSchedule>>;
