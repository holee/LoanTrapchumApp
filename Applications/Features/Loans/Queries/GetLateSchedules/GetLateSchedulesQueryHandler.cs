namespace Applications.Features.Loans.Queries.GetLateSchedules;

public sealed class GetLateSchedulesQueryHandler : IRequestHandler<GetLateSchedulesQuery, Pager<VSchedule>>
{
    private readonly ILoanScheduleService _loanScheduleService;
    public GetLateSchedulesQueryHandler(ILoanScheduleService loanScheduleService)
    {
        _loanScheduleService = loanScheduleService;
    }
    public async Task<Pager<VSchedule>> Handle(GetLateSchedulesQuery request, CancellationToken cancellationToken)
    {
        return await _loanScheduleService.GetLateSchedulesAsync(request.BranchId, request.LoanId, request.PaymentDate, cancellationToken);
    }
}
