using Application.Responses.ViewModels.Loans.Schedules;

namespace Application.Features.Loans.Queries.GetLoanSchedulesByPaymentDate;

public record GetLoanSchedulesMonthToDateQuery(int? BranchId, DateTimeOffset? StartDate, DateTimeOffset? EndDate, QueryParam? Param = null)
        : IRequest<Pager<VSchedule>>;
