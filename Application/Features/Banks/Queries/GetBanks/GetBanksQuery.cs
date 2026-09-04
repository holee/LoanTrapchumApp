using Application.Features.Banks.Dtos;
using Application.Utils;
using ErrorOr;

namespace Application.Features.Banks.Queries.GetBanks;

/// <summary>
/// Query to get all banks with optional search filter
/// </summary>
public sealed record GetBanksQuery(int? BranchId, QueryParam Query) : IRequest<PageMetaData<BankDto>>;
