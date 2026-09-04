using Application.Features.Schedules.Dto;
using ErrorOr;

namespace Application.Features.LoanPayments.Queries.GetScheduleById;

public sealed class GetScheduleByIdQueryHandler : IRequestHandler<GetScheduleByIdQuery, ErrorOr<ScheduleDetailsDto>>
{
    private readonly ILoanPaymentService _loanPaymentService;
    public GetScheduleByIdQueryHandler(ILoanPaymentService loanPaymentService)
    {
        _loanPaymentService = loanPaymentService;
    }
    public async Task<ErrorOr<ScheduleDetailsDto>> Handle(GetScheduleByIdQuery request, CancellationToken cancellationToken)
    {
        return await _loanPaymentService.GetScheduleByIdAsync(request.ScheduleId, request.BranchId, request.LoanId);
    }

}