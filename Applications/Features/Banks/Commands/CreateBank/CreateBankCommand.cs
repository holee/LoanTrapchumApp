using Applications.Features.Banks.Dtos;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Banks.Commands.CreateBank;

/// <summary>
/// Command to create a new Bank
/// </summary>
public sealed record CreateBankCommand(BankDtoCreate Request) : IRequest<ErrorOr<BankDto>>;
