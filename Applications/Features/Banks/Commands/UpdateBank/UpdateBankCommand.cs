using Applications.Features.Banks.Dtos;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Banks.Commands.UpdateBank;

/// <summary>
/// Command to update an existing Bank
/// </summary>
public sealed record UpdateBankCommand(int BankId, BankDtoUpdate Request) : IRequest<ErrorOr<BankDto>>;
