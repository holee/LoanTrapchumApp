using ErrorOr;

namespace Application.Features.Loans.Command.WidrawCollateral;

public sealed class WidrawalCollateralCommandHandler : IRequestHandler<WidrawalCollateralCommand, ErrorOr<bool>>
{
    private readonly ILoanService _loanService;
    public WidrawalCollateralCommandHandler(ILoanService loanService)
    {
        _loanService = loanService;
    }
    public async Task<ErrorOr<bool>> Handle(WidrawalCollateralCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _loanService.WidthrawalLoanCollateralsAsync(request.Request, cancellationToken);

            return result;
        }
        catch (Exception ex)
        {
            return Error.Failure(ex.Message);
        }

    }
}


