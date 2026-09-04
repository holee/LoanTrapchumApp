using Application.Features.Loans.Dtos.Responses;
using Application.Interfaces.Loans;
using Application.Utils;
using ErrorOr;

namespace Application.Features.LoanRequests.Command.RecheckedLoanRequest;

public sealed class RecheckedLoanRequestCommandHandler : IRequestHandler<RecheckedLoanRequestCommand, ErrorOr<RequestResult>>
{
    private readonly ILoanRequestService _reuqestService;

    public RecheckedLoanRequestCommandHandler(ILoanRequestService reuqestService)
    {
        _reuqestService = reuqestService;
    }

    public async Task<ErrorOr<RequestResult>> Handle(RecheckedLoanRequestCommand request, CancellationToken cancellationToken)
    {
        return await _reuqestService.RecheckedLoanRequestAsync(request.Request, cancellationToken);
    }
}
