using Application.Interfaces.Loans;
using Application.Utils;
using ErrorOr;

namespace Application.Features.LoanRequests.Command.AddLoanRequestCollateral;

public class AddLoanRequestCollateralCommandHandler : IRequestHandler<AddLoanRequestCollateralCommand, ErrorOr<string>>
{
    private readonly ILoanRequestService _loanRequestService;
    public AddLoanRequestCollateralCommandHandler(ILoanRequestService loanRequestService)
    {
        _loanRequestService = loanRequestService;
    }
    public async Task<ErrorOr<string>> Handle(AddLoanRequestCollateralCommand command, CancellationToken cancellationToken)
    {
        return await _loanRequestService.AddLoanRequestCollateralAsync(command.Request,cancellationToken);
    }
}
