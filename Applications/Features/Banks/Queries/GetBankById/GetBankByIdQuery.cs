using Applications.Features.Banks.Dtos;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Banks.Queries.GetBankById;

/// <summary>
/// Query to get a bank by its ID
/// </summary>
public sealed record GetBankByIdQuery(int BankId) : IRequest<ErrorOr<BankDto>>;
