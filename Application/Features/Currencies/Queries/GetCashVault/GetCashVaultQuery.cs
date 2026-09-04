using Application.Features.CashManagements.Responses;

namespace Application.Features.Currencies.Queries.GetCashVault;

public record GetCashVaultQuery(int? BranchId) : IRequest<Pager<VCashVaults>>;
