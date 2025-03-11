using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Currencies;
using Domains.Models.Transaction;

namespace Applications.Services.Transactions
{
    public interface IForiegnCurrencyExchange 
    {
        Task<Result<string>> Exchange(LoanTransaction transaction);
        Task<Pager<VttCurrencyExchanges>> GetCurrencyExchange(int branchId,int pageIndex, int pageSize, DateTime? transactionDate = null);
    }
}
