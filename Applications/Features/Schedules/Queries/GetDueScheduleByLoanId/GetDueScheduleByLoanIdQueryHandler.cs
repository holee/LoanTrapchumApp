namespace Applications.Features.Schedules.Queries.GetDueScheduleByLoanId;

public sealed class GetDueScheduleByLoanIdQueryHandler : IRequestHandler<GetDueScheduleByLoanIdQuery, Pager<VSchedule>>
{
    private readonly ILoanService _scheduleRepository;
    public GetDueScheduleByLoanIdQueryHandler(ILoanService scheduleRepository)
    {
        _scheduleRepository = scheduleRepository;
    }
    public async Task<Pager<VSchedule>> Handle(GetDueScheduleByLoanIdQuery request, CancellationToken cancellationToken)
    {
        return await _scheduleRepository.GetScheduleLoanByIdAsync(request.LoanId, request.BranchId, request.Status, cancellationToken);
    }
}