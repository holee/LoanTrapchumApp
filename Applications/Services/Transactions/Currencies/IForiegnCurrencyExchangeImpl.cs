using Applications.Requests.Transactions.ForiegnExchanges;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Currencies;
using Domains.Models.Transaction;

namespace Applications.Services.Transactions.Currencies
{
    public interface IForiegnCurrencyExchangeImpl
    {
        Task<Result<string>> Exchange(TrxForeignExchangeDtoCreate transaction);
        Task<Pager<VttCurrencyExchanges>> GetCurrencyExchange(int branchId, int pageIndex, int pageSize, DateTime? transactionDate = null);
    }
}
