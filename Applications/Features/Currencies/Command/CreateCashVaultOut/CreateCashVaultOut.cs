using ErrorOr;
using Applications.Features.Transactions.CashManagements;
namespace Applications.Features.Currencies.Command.CreateCashVaultOut;  

public record CreateCashVaultOut(TrnxCashVaultDtoCreate TrnxCashVault) : IRequest<ErrorOr<string>>;
