using Application.Features.Loans.Dtos.Responses;
using ErrorOr;

namespace Application.Features.Schedules.GetLoanWithSchedule;

public record GetLoanWithScheduleQuery(int? BranchId, string? LoanId) : IRequest<ErrorOr<LoanResponse>>;
