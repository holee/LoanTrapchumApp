using Applications.Interfaces.Loans;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Loans.Schedules;
using Applications.Utils;

namespace Applications.Features.LoanPayments.Queries.GetLoanSchedulesByLoanIdAsPaging;

public sealed class GetLoanSchedulesByLoanIdAsPagingQueryHandler(ILoanScheduleService loanScheduleService)
    : IRequestHandler<GetLoanSchedulesByLoanIdAsPagingQuery, Pager<VSchedule>>
{
    public async Task<Pager<VSchedule>> Handle(GetLoanSchedulesByLoanIdAsPagingQuery request, CancellationToken cancellationToken)
    {
        return await loanScheduleService.GetLoanScheduleByIdAsync(request.LoanId, request.BranchId, request.Status, request.Param);
    }
}
 