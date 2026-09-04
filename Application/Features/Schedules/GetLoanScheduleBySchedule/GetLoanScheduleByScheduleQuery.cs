using Application.Features.Loans.Dtos.Responses;
using ErrorOr;

namespace Application.Features.Schedules.GetLoanScheduleBySchedule;

public record GetLoanScheduleByScheduleQuery(int? BranchId, string? LoanId,int? ScheduleNum) : IRequest<ErrorOr<LoanResponse>>;
