using Applications.Enumerations;
using Applications.Interfaces.Transactions;
using Domains.Models.Transaction;
using ErrorOr;
using Mapster;

namespace Applications.Features.Currencies.Command.CreateCashVaultIn;

public sealed class CreateCashVaultInHandler : IRequestHandler<CreateCashVaultIn, ErrorOr<string>>
{
    private readonly ICashVaultService _startVaultInUseCase;
    public CreateCashVaultInHandler(ICashVaultService startVaultInUseCase)
    {
        _startVaultInUseCase = startVaultInUseCase;
    }
    public async Task<ErrorOr<string>> Handle(CreateCashVaultIn request, CancellationToken cancellationToken)
    {

        var req = request.TrnxCashVault.Adapt<LoanTransaction>();

        req.TranTypeRefId = (int)TransactionTypes.CSV;

        return await _startVaultInUseCase.CashVaultAsync(req, cancellationToken);
    }
}
