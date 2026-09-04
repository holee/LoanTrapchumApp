using Application.Enumerations;
using Application.Responses.Paginations;
using Application.Responses.ViewModels.Loans.Schedules;
using Application.Utils;

namespace Application.Features.LoanPayments.Queries.GetLoanSchedulesByLoanIdAsPaging;

public record GetLoanSchedulesByLoanIdAsPagingQuery(string? LoanId, int? BranchId, ScheduleStatus Status = ScheduleStatus.All, QueryParam? Param = null)
    : IRequest<Pager<VSchedule>>;
