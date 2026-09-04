using Application.Features.Currencies.Responses;
using Application.Features.Transactions.ForiegnExchanges;
using Application.Interfaces.Transactions;
using Application.Responses;
using Domains.Models.Transaction;
using Mapster;

namespace Application.Services.Transactions.Currencies
{
    public sealed class ForiegnCurrencyExchange(IForiegnCurrencyExchange frExchange) : IForiegnCurrencyExchangeImpl
    {

        public async Task<Result<string>> Exchange(TrxForeignExchangeDtoCreate transaction)
        {

            var model = transaction.Adapt<LoanTransaction>();
            return await frExchange.Exchange(model);
        }

        public async Task<Pager<VttCurrencyExchanges>> GetCurrencyExchange(int branchId, int pageIndex, int pageSize, DateTime? transactionDate = null)
        {
            return await GetCurrencyExchange(branchId, pageIndex, pageSize, transactionDate);
        }
    }
}
