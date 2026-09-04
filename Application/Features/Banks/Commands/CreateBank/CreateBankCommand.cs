using Application.Features.Banks.Dtos;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Banks.Commands.CreateBank;

/// <summary>
/// Command to create a new Bank
/// </summary>
public sealed record CreateBankCommand(BankDtoCreate Request) : IRequest<ErrorOr<BankDto>>;
