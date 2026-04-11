using ErrorOr;

namespace Applications.Features.LoanPayments.Queries.GetPaidSchedule;

public sealed record GetPaidScheduleQueryHandler(ILoanPaymentService LoanPaymentService) : IRequestHandler<GetPaidScheduleQuery, ErrorOr<IEnumerable<VSchedule>>>
{
    public async Task<ErrorOr<IEnumerable<VSchedule>>> Handle(GetPaidScheduleQuery request, CancellationToken cancellationToken)
    {
        return await LoanPaymentService.GetPaidScheduleAsync(request.BranchId, request.LoanId);
    }
}
