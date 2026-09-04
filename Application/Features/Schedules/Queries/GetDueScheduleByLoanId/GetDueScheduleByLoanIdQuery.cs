using Application.Enumerations;

namespace Application.Features.Schedules.Queries.GetDueScheduleByLoanId;

public record GetDueScheduleByLoanIdQuery(int? BranchId, string? LoanId, ScheduleStatus Status = ScheduleStatus.Unpaid, DateTime? DueDate = null) : IRequest<Pager<VSchedule>>;
