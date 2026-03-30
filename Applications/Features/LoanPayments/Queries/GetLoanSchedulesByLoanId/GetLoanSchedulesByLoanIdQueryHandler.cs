using ErrorOr;

namespace Applications.Features.LoanPayments.Queries.GetLoanSchedulesByLoanIdAsPaging;

public sealed class GetLoanSchedulesByLoanIdQueryHandler(ILoanScheduleService loanScheduleService)
    : IRequestHandler<GetLoanSchedulesByLoanIdQuery, ErrorOr<IEnumerable<VSchedule>>>
{

    public async Task<ErrorOr<IEnumerable<VSchedule>>> Handle(GetLoanSchedulesByLoanIdQuery request, CancellationToken cancellationToken)
    {
        var result = await loanScheduleService.GetLoanScheduleByIdAsync(request.LoanId, request.BranchId, request.Status, cancellationToken);

        return result.ToList();
    }

}

