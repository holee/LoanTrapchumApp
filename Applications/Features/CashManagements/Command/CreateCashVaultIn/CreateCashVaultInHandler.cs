using Applications.Enumerations;
using Applications.Interfaces.Transactions;
using Domains.Models.Transaction;
using ErrorOr;
using Mapster;

namespace Applications.Features.CashManagements.Command.CreateCashVaultIn;

public sealed class CreateCashVaultInHandler : IRequestHandler<StartCashVaultIn, ErrorOr<string>>
{
    private readonly ICashVaultService _startVaultInUseCase;
    public CreateCashVaultInHandler(ICashVaultService startVaultInUseCase)
    {
        _startVaultInUseCase = startVaultInUseCase;
    }
    public async Task<ErrorOr<string>> Handle(StartCashVaultIn request, CancellationToken cancellationToken)
    {

        var req = request.TrnxCashVault.Adapt<LoanTransaction>();

        req.TranTypeRefId = (int)TransactionTypes.CSV;

        return await _startVaultInUseCase.CashVaultAsync(req, cancellationToken);
    }
}
