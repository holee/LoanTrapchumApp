using Applications.Responses.ViewModels.Loans.Schedules;

namespace Applications.Features.Loans.Queries.GetLoanSchedulesByPaymentDate;

public sealed class GetLoanSchedulesByPaymentDateQueryHandler(ILoanScheduleService loanScheduleService)
    : IRequestHandler<GetLoanSchedulesByPaymentDateQuery, Pager<VSchedule>>
{
    public async Task<Pager<VSchedule>> Handle(GetLoanSchedulesByPaymentDateQuery request, CancellationToken cancellationToken = default)
    {
        return await loanScheduleService.GetLoanSchedulesByPaymentDateAsync(request.PaymentDate, request.BranchId, request.IsIncludeLate, request.IsLateOnly, request.Param);
    }
}