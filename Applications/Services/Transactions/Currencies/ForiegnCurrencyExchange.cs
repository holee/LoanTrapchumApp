using Applications.Features.Currencies.Responses;
using Applications.Features.Transactions.ForiegnExchanges;
using Applications.Features.Transactions.ForiegnExchanges;
using Applications.Interfaces.Transactions;
using Applications.Responses;
using Applications.Responses.Paginations;
using Domains.Models.Transaction;
using Mapster;

namespace Applications.Services.Transactions.Currencies
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
