using Applications.Features.Banks.Dtos;
using Applications.Utils;
using ErrorOr;

namespace Applications.Features.Banks.Queries.GetBanks;

/// <summary>
/// Query to get all banks with optional search filter
/// </summary>
public sealed record GetBanksQuery(int? BranchId, QueryParam Query) : IRequest<PageMetaData<BankDto>>;
