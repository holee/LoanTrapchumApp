using Applications.Enumerations;

namespace Applications.Features.Schedules.Queries.GetDueScheduleByLoanId;

public record GetDueScheduleByLoanIdQuery(int? BranchId, string? LoanId, ScheduleStatus Status = ScheduleStatus.Unpaid) : IRequest<Pager<VSchedule>>;
