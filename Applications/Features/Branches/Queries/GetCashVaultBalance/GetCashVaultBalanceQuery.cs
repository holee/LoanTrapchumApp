using Applications.Features.CashManagements.Responses;
using ErrorOr;

namespace Applications.Features.Branches.Queries.GetCashVaultBalance;

public record GetCashVaultBalanceQuery(int? BranchId) : IRequest<ErrorOr<IEnumerable<VCashVaultBalance>>>;
