using Applications.Features.Transactions.CashManagements;
using ErrorOr;

namespace Applications.Features.Currencies.Command.CreateCashVaultIn;  

public record CreateCashVaultIn(TrnxCashVaultDtoCreate TrnxCashVault) : IRequest<ErrorOr<string>>;
