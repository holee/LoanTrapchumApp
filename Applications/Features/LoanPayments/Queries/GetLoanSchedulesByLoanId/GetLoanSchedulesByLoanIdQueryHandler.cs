using Applications.Interfaces.Loans;
using Applications.Responses.ViewModels.Loans.Schedules;
using Applications.Utils;
using ErrorOr;
using System.Collections;

namespace Applications.Features.LoanPayments.Queries.GetLoanSchedulesByLoanIdAsPaging;

public sealed class GetLoanSchedulesByLoanIdQueryHandler(ILoanScheduleService loanScheduleService)
    : IRequestHandler<GetLoanSchedulesByLoanIdQuery, ErrorOr<IEnumerable<VSchedule>>>
{

    public async Task<ErrorOr<IEnumerable<VSchedule>>> Handle(GetLoanSchedulesByLoanIdQuery request, CancellationToken cancellationToken)
    {
        var result = await loanScheduleService.GetLoanScheduleByIdAsync(request.LoanId, request.BranchId, request.Status);
        return result.IsSuccess && result.Data is not null
            ? ErrorOrFactory.From(result.Data)
            : ErrorOrFactory.From(Enumerable.Empty<VSchedule>());
    }

}
