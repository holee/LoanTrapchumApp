using Application.Features.Transactions.CashManagements;
using ErrorOr;

namespace Application.Features.CashManagements.Command.CreateCashVaultIn;  

public record StartCashVaultIn(TrnxCashVaultDtoCreate TrnxCashVault) : IRequest<ErrorOr<string>>;
