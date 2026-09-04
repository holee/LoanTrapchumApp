using Application.Features.Loans.Dtos.Responses;
using Application.Interfaces;
using ErrorOr;

namespace Application.Features.Schedules.GetLoanWithSchedule;

public sealed class GetLoanWithScheduleQueryHandler : IRequestHandler<GetLoanWithScheduleQuery, ErrorOr<LoanResponse>>
{
    private readonly IScheduleService _loanService;
    public GetLoanWithScheduleQueryHandler(IScheduleService loanService)
    {
        _loanService = loanService;
    }
    public async Task<ErrorOr<LoanResponse>> Handle(GetLoanWithScheduleQuery request, CancellationToken cancellationToken)
    {
        return await _loanService.CreatePdf(request.BranchId, request.LoanId, cancellationToken);
    }
}