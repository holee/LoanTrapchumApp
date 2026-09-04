using ErrorOr;
using Application.Features.Transactions.CashManagements;
namespace Application.Features.CashManagements.Command.CreateCashVaultOut;  

public record StartCashVaultOut(TrnxCashVaultDtoCreate TrnxCashVault) : IRequest<ErrorOr<string>>;
