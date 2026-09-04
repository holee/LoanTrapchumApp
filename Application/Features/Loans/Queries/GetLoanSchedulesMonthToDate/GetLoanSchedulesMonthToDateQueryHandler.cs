using Application.Responses.ViewModels.Loans.Schedules;

namespace Application.Features.Loans.Queries.GetLoanSchedulesByPaymentDate;

public sealed class GetLoanSchedulesMonthToDateQueryHandler(ILoanScheduleService loanScheduleService)
    : IRequestHandler<GetLoanSchedulesMonthToDateQuery, Pager<VSchedule>>
{
    public async Task<Pager<VSchedule>> Handle(GetLoanSchedulesMonthToDateQuery request, CancellationToken cancellationToken = default)
    {
        return await loanScheduleService.GetLoanSchedulesMonthToDateAsync(request.BranchId, request.StartDate, request.EndDate, request.Param);
    }
}
