using Application.Features.CashManagements.Responses;
using Application.Interfaces.Transactions;
using ErrorOr;

namespace Application.Features.Branches.Queries.GetCashVaultBalance;

public sealed class GetCashVaultBalanceQueryHandler(IOpenBranchService obService) : IRequestHandler<GetCashVaultBalanceQuery, ErrorOr<IEnumerable<VCashVaultBalance>>>
{
    public async Task<ErrorOr<IEnumerable<VCashVaultBalance>>> Handle(GetCashVaultBalanceQuery request, CancellationToken cancellationToken)
    {
        return await obService.GetCashVaultBalance(request.BranchId);
    }

}
