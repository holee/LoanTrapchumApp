using Application.Features.CashManagements.Responses;
using ErrorOr;

namespace Application.Features.Branches.Queries.GetCashVaultBalance;

public record GetCashVaultBalanceQuery(int? BranchId) : IRequest<ErrorOr<IEnumerable<VCashVaultBalance>>>;
