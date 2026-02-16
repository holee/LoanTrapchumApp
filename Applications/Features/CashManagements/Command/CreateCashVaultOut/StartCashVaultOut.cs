using ErrorOr;
using Applications.Features.Transactions.CashManagements;
namespace Applications.Features.CashManagements.Command.CreateCashVaultOut;  

public record StartCashVaultOut(TrnxCashVaultDtoCreate TrnxCashVault) : IRequest<ErrorOr<string>>;
