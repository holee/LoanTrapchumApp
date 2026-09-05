using Applications.Features.Currencies.Responses;
using Applications.Interfaces.Transactions;
using Applications.Responses;
using Applications.Responses.Paginations;
using Domains.Models.Transaction;
using ErrorOr;

namespace Web.UI.Services;

/// <summary>
/// WebAssembly stand-in for the EF-backed <c>ForiegnCurrencyExchange</c> service.
/// Exchange writes are rejected client-side; reads return empty results.
/// </summary>
public sealed class ForiegnCurrencyExchangeClient(HttpClient httpClient) : IForiegnCurrencyExchange
{
    public Task<ErrorOr<string>> CreateExchangeAsync(LoanTransaction transaction, CancellationToken cancellation = default)
        => Task.FromResult<ErrorOr<string>>(NotAvailable);

    public Task<ErrorOr<VttCurrencyExchanges>> CreateExchangeAsync(ForeignExchange transaction, CancellationToken cancellation = default)
        => Task.FromResult<ErrorOr<VttCurrencyExchanges>>(NotAvailable);

    public Task<ErrorOr<VttCurrencyExchanges>> EditExchangeAsync(ForeignExchange transaction, CancellationToken cancellation = default)
        => Task.FromResult<ErrorOr<VttCurrencyExchanges>>(NotAvailable);

    public Task<Result<string>> Exchange(LoanTransaction transaction)
        => Task.FromResult(Applications.Responses.Result.Failure<string>(default(string)!));

    public Task<Pager<VttCurrencyExchanges>> GetCurrenciesExchange(int? branchId, DateTimeOffset? transactionDate, QueryParam param, CancellationToken cancellation = default)
        => Task.FromResult(new Pager<VttCurrencyExchanges>([]));

    public Task<VttCurrencyExchanges> GetCurrencyExchange(int? branchId, string? transactionId, DateTimeOffset? transactionDate, CancellationToken cancellation = default)
        => Task.FromResult(new VttCurrencyExchanges());

    private static Error NotAvailable
        => Error.Unexpected(description: "Currency exchange is not available from the browser client.");
}
