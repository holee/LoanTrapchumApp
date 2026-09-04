using Application.Features.Currencies.Responses;
using Application.Features.Transactions.ForiegnExchanges;
using Application.Responses;
using Application.Responses.Paginations;
using Domains.Models.Transaction;

namespace Application.Services.Transactions.Currencies
{
    public interface IForiegnCurrencyExchangeImpl
    {
        Task<Result<string>> Exchange(TrxForeignExchangeDtoCreate transaction);
        Task<Pager<VttCurrencyExchanges>> GetCurrencyExchange(int branchId, int pageIndex, int pageSize, DateTime? transactionDate = null);
    }
}
