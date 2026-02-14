using Applications.Features.CashManagements.Responses;
using Applications.Interfaces.Transactions;

namespace Applications.Features.Currencies.Queries.GetCashVault;

public sealed class GetCashVaultQueryHandler : IRequestHandler<GetCashVaultQuery, Pager<VCashVaults>>
{
    private readonly ICashVaultService _cashVaultService;
    public GetCashVaultQueryHandler(ICashVaultService cashVaultService)
    {
        _cashVaultService = cashVaultService;
    }
    public async Task<Pager<VCashVaults>> Handle(GetCashVaultQuery request, CancellationToken cancellationToken)
    {
        return await _cashVaultService.GetViewCashVaultAsync(request.BranchId, cancellationToken);
    }
}