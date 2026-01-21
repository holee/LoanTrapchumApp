using Applications.Features.Currencies.Responses;
using Applications.Responses;
using Domains.Models.Transaction;
using ErrorOr;

namespace Applications.Interfaces.Transactions
{
    public interface IForiegnCurrencyExchange
    {
        Task<ErrorOr<string>> CreateExchangeAsync(LoanTransaction transaction, CancellationToken cancellation = default);
        Task<ErrorOr<VttCurrencyExchanges>> CreateExchangeAsync(ForeignExchange transaction, CancellationToken cancellation = default);
        Task<ErrorOr<VttCurrencyExchanges>> EditExchangeAsync(ForeignExchange transaction, CancellationToken cancellation = default);
        Task<Result<string>> Exchange(LoanTransaction transaction);
        Task<Pager<VttCurrencyExchanges>> GetCurrenciesExchange(int? branchId, DateTimeOffset? transactionDate, QueryParam param, CancellationToken cancellation = default);
        Task<VttCurrencyExchanges> GetCurrencyExchange(int? branchId, string? transactionId, DateTimeOffset? transactionDate, CancellationToken cancellation = default);
    }
}
