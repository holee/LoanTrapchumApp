using Applications.Enumerations;
using Applications.Interfaces.Transactions;
using Domains.Models.Transaction;
using ErrorOr;
using Mapster;

namespace Applications.Features.CashManagements.Command.CreateCashVaultOut;

public sealed class StartCashVaultOutHandler : IRequestHandler<StartCashVaultOut, ErrorOr<string>>
{
    private readonly ICashVaultService _startVaultOutUseCase;
    public StartCashVaultOutHandler(ICashVaultService startVaultOutUseCase)
    {
        _startVaultOutUseCase = startVaultOutUseCase;
    }
    public async Task<ErrorOr<string>> Handle(StartCashVaultOut request, CancellationToken cancellationToken)
    {

        var req = request.TrnxCashVault.Adapt<LoanTransaction>();

        req.TranTypeRefId = (int)TransactionTypes.CSV;

        return await _startVaultOutUseCase.CashVaultAsync(req, cancellationToken);   
    }
}
