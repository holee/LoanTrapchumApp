using Application.Features.CashManagements.Responses;
using Application.Interfaces.Transactions;
using Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CashManagements.UseCases
{
    public sealed class GetCashVaultUseCase : IGetCashVaultUseCase
    {
        private readonly ICashVaultService _cashService;

        public GetCashVaultUseCase(ICashVaultService cashService)
        {
            _cashService = cashService;
        }


        public async Task<Result<IEnumerable<VCashVaults>>> ExecuteAsync(int? branchId)
        {
            if (branchId.HasValue)
            {
                return await _cashService.GetCashVaultsAsync(branchId);
            }

            return Result.Failure<IEnumerable<VCashVaults>>(ErrorResponse.ServerError("Branch Id Is Required."));
        }
    }
}
