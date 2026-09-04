using Application.Features.CashManagements.Responses;
using Application.Interfaces.Transactions;

namespace Application.Features.Currencies.Queries.GetCashVault;

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