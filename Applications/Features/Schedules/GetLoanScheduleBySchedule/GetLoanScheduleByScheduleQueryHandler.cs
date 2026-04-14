using Applications.Features.Loans.Dtos.Responses;
using Applications.Interfaces;
using ErrorOr;

namespace Applications.Features.Schedules.GetLoanScheduleBySchedule;

public sealed class GetLoanScheduleByScheduleQueryHandler : IRequestHandler<GetLoanScheduleByScheduleQuery, ErrorOr<LoanResponse>>
{
    private readonly IScheduleService _loanService;
    public GetLoanScheduleByScheduleQueryHandler(IScheduleService loanService)
    {
        _loanService = loanService;
    }
    public async Task<ErrorOr<LoanResponse>> Handle(GetLoanScheduleByScheduleQuery request, CancellationToken cancellationToken)
    {
        return await _loanService.GetLoanWithSchedules(request.BranchId, request.LoanId, request.ScheduleNum, cancellationToken);
    }
}