namespace Applications.Features.Schedules.Queries.GetDueScheduleByLoanId;

public record GetDueScheduleByLoanIdQuery(int? BranchId, string? LoanId) : IRequest<Pager<VSchedule>>;
