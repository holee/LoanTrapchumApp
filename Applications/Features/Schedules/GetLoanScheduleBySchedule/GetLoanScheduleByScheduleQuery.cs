using ErrorOr;

namespace Applications.Features.Schedules.GetLoanScheduleBySchedule;

public record GetLoanScheduleByScheduleQuery(int? BranchId, string? LoanId,int? ScheduleNum) : IRequest<ErrorOr<LoanResponse>>;
