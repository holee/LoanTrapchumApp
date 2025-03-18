using Applications.Interfaces.Transactions;
using Applications.Requests.Transactions.ForiegnExchanges;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Currencies;
using Domains.Models.Transaction;

namespace Applications.Services.Transactions.Currencies
{
    public sealed class ForiegnCurrencyExchange(IForiegnCurrencyExchange frExchange) : IForiegnCurrencyExchange
    {

        public async Task<Result<string>> Exchange(LoanTransaction transaction)
        {
            return await frExchange.Exchange(transaction);
        }

        public async Task<Pager<VttCurrencyExchanges>> GetCurrencyExchange(int branchId, int pageIndex, int pageSize, DateTime? transactionDate = null)
        {
            return await GetCurrencyExchange(branchId, pageIndex, pageSize, transactionDate);
        }
    }
}
