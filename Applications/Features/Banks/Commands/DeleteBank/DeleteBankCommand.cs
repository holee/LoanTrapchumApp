using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Banks.Commands.DeleteBank;

/// <summary>
/// Command to delete an existing Bank
/// </summary>
public sealed record DeleteBankCommand(int BankId) : IRequest<ErrorOr<bool>>;
