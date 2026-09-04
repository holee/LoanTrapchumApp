using Application.Enumerations;
using Application.Responses.Paginations;
using Application.Responses.ViewModels.Loans.Schedules;
using Application.Utils;
using ErrorOr;

namespace Application.Features.LoanPayments.Queries.GetLoanSchedulesByLoanIdAsPaging;

public record GetLoanSchedulesByLoanIdQuery(string? LoanId, int? BranchId, ScheduleStatus Status = ScheduleStatus.All, QueryParam? Param = null)
    : IRequest<ErrorOr<IEnumerable<VSchedule>>>;
