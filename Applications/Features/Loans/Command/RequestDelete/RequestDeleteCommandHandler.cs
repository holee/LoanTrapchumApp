using ErrorOr;

namespace Applications.Features.Loans.Command.RequestDelete;

public class RequestDeleteCommandHandler : IRequestHandler<RequestDeleteCommand,ErrorOr<DeleteResult>>
{
    private readonly ILoanService _loanService;
    public RequestDeleteCommandHandler(ILoanService loanService)
    {
        _loanService = loanService;
    }
    public async Task<ErrorOr<DeleteResult>> Handle(RequestDeleteCommand request, CancellationToken cancellationToken)
    {
        return await _loanService.RequestDeleteLoanAsync(request.Request, cancellationToken);
    }
}

