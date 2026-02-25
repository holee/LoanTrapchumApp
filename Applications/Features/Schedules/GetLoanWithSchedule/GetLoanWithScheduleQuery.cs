using ErrorOr;

namespace Applications.Features.Schedules.GetLoanWithSchedule;

public record GetLoanWithScheduleQuery(int? BranchId, string? LoanId) : IRequest<ErrorOr<LoanResponse>>;
