using Applications.Features.Currencies.Responses;
using Applications.Features.Transactions.ForiegnExchanges;
using Applications.Responses;
using Applications.Responses.Paginations;
using Domains.Models.Transaction;

namespace Applications.Services.Transactions.Currencies
{
    public interface IForiegnCurrencyExchangeImpl
    {
        Task<Result<string>> Exchange(TrxForeignExchangeDtoCreate transaction);
        Task<Pager<VttCurrencyExchanges>> GetCurrencyExchange(int branchId, int pageIndex, int pageSize, DateTime? transactionDate = null);
    }
}
