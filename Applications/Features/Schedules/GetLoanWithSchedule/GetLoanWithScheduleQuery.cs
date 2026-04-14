using Applications.Features.Loans.Dtos.Responses;
using ErrorOr;

namespace Applications.Features.Schedules.GetLoanWithSchedule;

public record GetLoanWithScheduleQuery(int? BranchId, string? LoanId) : IRequest<ErrorOr<LoanResponse>>;
