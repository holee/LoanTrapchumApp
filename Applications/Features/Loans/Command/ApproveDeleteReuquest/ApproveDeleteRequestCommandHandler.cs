using ErrorOr;

namespace Applications.Features.Loans.Command.ApproveDeleteReuquest;

public sealed class ApproveDeleteRequestCommandHandler : IRequestHandler<ApproveDeleteRequestCommand, ErrorOr<DisburseResult>>
{
    private readonly ILoanService _loanService;
    public ApproveDeleteRequestCommandHandler(ILoanService loanService)
    {
        _loanService = loanService;
    }
    public async Task<ErrorOr<DisburseResult>> Handle(ApproveDeleteRequestCommand request, CancellationToken cancellationToken)
    {
        try
        {
            return await _loanService.ApproveDeleteRequestAsync(request.Request, cancellationToken);
        }
        catch (Exception ex)
        {
            return Error.Unexpected(description: "An unexpected error occurred while approving the delete request: " + ex.Message);
        }

    }
}