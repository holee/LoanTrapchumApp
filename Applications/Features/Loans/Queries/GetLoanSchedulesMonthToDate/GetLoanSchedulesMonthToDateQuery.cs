using Applications.Responses.ViewModels.Loans.Schedules;

namespace Applications.Features.Loans.Queries.GetLoanSchedulesByPaymentDate;

public record GetLoanSchedulesMonthToDateQuery(int? BranchId, DateTimeOffset? StartDate, DateTimeOffset? EndDate, QueryParam? Param = null)
        : IRequest<Pager<VSchedule>>;
