using ErrorOr;

namespace Applications.Features.Loans.Command.RequestEditLoan;

public sealed class RequestEditLoanCommandHandler(ILoanService loanService)
    : IRequestHandler<RequestEditLoanCommand, ErrorOr<AppResult>>
{
    public async Task<ErrorOr<AppResult>> Handle(RequestEditLoanCommand request, CancellationToken cancellationToken)
    {
        try
        {
           return  await loanService.ApproveEditLoanRequestAsync(request.Request,cancellationToken);
        }
        catch (Exception e)
        {
            return Error.Unexpected(description: e.Message);
        }
      
    }
}