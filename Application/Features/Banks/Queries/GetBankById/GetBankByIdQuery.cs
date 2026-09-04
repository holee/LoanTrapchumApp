using Application.Features.Banks.Dtos;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Banks.Queries.GetBankById;

/// <summary>
/// Query to get a bank by its ID
/// </summary>
public sealed record GetBankByIdQuery(int BankId) : IRequest<ErrorOr<BankDto>>;
