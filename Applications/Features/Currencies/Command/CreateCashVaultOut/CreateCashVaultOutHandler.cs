using Applications.Enumerations;
using Applications.Interfaces.Transactions;
using Domains.Models.Transaction;
using ErrorOr;
using Mapster;

namespace Applications.Features.Currencies.Command.CreateCashVaultOut;

public sealed class CreateCashVaultOutHandler : IRequestHandler<CreateCashVaultOut, ErrorOr<string>>
{
    private readonly ICashVaultService _startVaultOutUseCase;
    public CreateCashVaultOutHandler(ICashVaultService startVaultOutUseCase)
    {
        _startVaultOutUseCase = startVaultOutUseCase;
    }
    public async Task<ErrorOr<string>> Handle(CreateCashVaultOut request, CancellationToken cancellationToken)
    {

        var req = request.TrnxCashVault.Adapt<LoanTransaction>();

        req.TranTypeRefId = (int)TransactionTypes.CSV;

        return await _startVaultOutUseCase.CashVaultAsync(req, cancellationToken);   
    }
}
