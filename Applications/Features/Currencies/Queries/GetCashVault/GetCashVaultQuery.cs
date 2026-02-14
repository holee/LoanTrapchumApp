using Applications.Features.CashManagements.Responses;

namespace Applications.Features.Currencies.Queries.GetCashVault;

public record GetCashVaultQuery(int? BranchId) : IRequest<Pager<VCashVaults>>;
