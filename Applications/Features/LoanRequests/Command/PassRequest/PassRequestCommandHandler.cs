using ErrorOr;

namespace Applications.Features.LoanRequests.Command.PassRequest;

public sealed class PassRequestCommandHandler : IRequestHandler<PassRequestCommand, ErrorOr<RequestResult>>
{
    private readonly ILoanRequestService _loanRequestService;
    public PassRequestCommandHandler(ILoanRequestService loanRequestService)
    {
        _loanRequestService = loanRequestService;
    }
    public async Task<ErrorOr<RequestResult>> Handle(PassRequestCommand request, CancellationToken cancellationToken)
    {                   
        return await _loanRequestService.PassLoanRequestAsync(request.Request,cancellationToken);
    }
}

