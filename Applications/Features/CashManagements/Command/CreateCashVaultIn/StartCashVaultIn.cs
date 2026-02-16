using Applications.Features.Transactions.CashManagements;
using ErrorOr;

namespace Applications.Features.CashManagements.Command.CreateCashVaultIn;  

public record StartCashVaultIn(TrnxCashVaultDtoCreate TrnxCashVault) : IRequest<ErrorOr<string>>;
