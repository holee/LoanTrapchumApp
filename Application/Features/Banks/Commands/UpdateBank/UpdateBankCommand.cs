using Application.Features.Banks.Dtos;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Banks.Commands.UpdateBank;

/// <summary>
/// Command to update an existing Bank
/// </summary>
public sealed record UpdateBankCommand(int BankId, BankDtoUpdate Request) : IRequest<ErrorOr<BankDto>>;
