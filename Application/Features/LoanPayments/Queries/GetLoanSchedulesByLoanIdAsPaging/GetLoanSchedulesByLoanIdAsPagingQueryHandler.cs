using Application.Interfaces.Loans;
using Application.Responses.Paginations;
using Application.Responses.ViewModels.Loans.Schedules;
using Application.Utils;

namespace Application.Features.LoanPayments.Queries.GetLoanSchedulesByLoanIdAsPaging;

public sealed class GetLoanSchedulesByLoanIdAsPagingQueryHandler(ILoanScheduleService loanScheduleService)
    : IRequestHandler<GetLoanSchedulesByLoanIdAsPagingQuery, Pager<VSchedule>>
{
    public async Task<Pager<VSchedule>> Handle(GetLoanSchedulesByLoanIdAsPagingQuery request, CancellationToken cancellationToken)
    {
        return await loanScheduleService.GetLoanScheduleByIdAsync(request.LoanId, request.BranchId, request.Status, request.Param);
    }
}
 