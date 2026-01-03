using Applications.Exceptions;
using Applications.Features.Currencies.Responses;
using Applications.Interfaces.Transactions;
using Applications.Responses;
using Applications.Responses.Paginations;
using Domains.Models.Transaction;
using Infrastructures.DataAccess;
using Infrastructures.Implements.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Implements
{
    public sealed class ForiegnCurrencyExchange : IForiegnCurrencyExchange
    {
        private readonly DataContext _context;
        private readonly IDbContextFactory<DataContext> _contextFactory;
        public ForiegnCurrencyExchange(DataContext context, IDbContextFactory<DataContext> contextFactory)
        {
            _context = context;
            _contextFactory = contextFactory;
        }

        public async Task<Result<string>> Exchange(LoanTransaction transaction)
        {
            try
            {
                _context.LoanTransaction.Add(transaction);

                await _context.SaveChangesAsync();

                return Result.Success(new string("Exchange Success."));
            }
            catch (Exception ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
            }
        }


        public async Task<ErrorOr.ErrorOr<string>> CreateExchangeAsync(LoanTransaction transaction, CancellationToken cancellation = default)
        {
            try
            {
                _context.LoanTransaction.Add(transaction);

                await _context.SaveChangesAsync(cancellation);

                return transaction.TransactionsId;
            }
            catch (DbUpdateException ex)
            {
                return ErrorOr.Error.Failure("Error", ex.Message);
            }
        }

        public async Task<ErrorOr.ErrorOr<string>> EditExchangeAsync(LoanTransaction transaction, CancellationToken cancellation = default)
        {
            await using var context = await _contextFactory.CreateDbContextAsync(cancellation);

            try
            {
                context.LoanTransaction.Update(transaction);

                await context.SaveChangesAsync(cancellation);

                return transaction.TransactionsId;
            }
            catch (DbUpdateException ex)
            {
                return ErrorOr.Error.Failure("Error", ex.Message);
            }
        }


        public async Task<VttCurrencyExchanges> GetCurrencyExchange(int? branchId, string? transactionId, DateTimeOffset? transactionDate, CancellationToken cancellation = default)
        {

            await using var context = await _contextFactory.CreateDbContextAsync(cancellation);

            var result = await context.Database
                                .SqlQuery<VttCurrencyExchanges>($"SELECT * FROM VTTExchangeTransactions WHERE branchId={branchId} AND transactionsId={transactionId} AND CAST(TransactionDate AS DATE)={transactionDate!.Value.Date}")
                                .AsNoTracking()
                                .FirstOrDefaultAsync(cancellation);

            if (result is null)
            {
                throw new CurrencyNotFoundException();
            }

            return result;
        }

        public async Task<Pager<VttCurrencyExchanges>> GetCurrenciesExchange(int? branchId, DateTimeOffset? transactionDate, QueryParam param, CancellationToken cancellation = default)
        {
            await using var context = await _contextFactory.CreateDbContextAsync(cancellation);

            var queries = context.Database
                                .SqlQuery<VttCurrencyExchanges>($"SELECT * FROM VTTExchangeTransactions WHERE branchId={branchId}")
                                .OrderByDescending(x => x.TransactionDate)
                                .AsNoTracking();

            if (transactionDate.HasValue)
            {
                queries = queries.Where(x => x.TransactionDate == transactionDate);
            }

            if (!string.IsNullOrEmpty(param.FilterColumn))
            {
                queries = queries.ApplyOrdering(param);
            }

            var totalRecords = await queries.CountAsync(cancellation);
            var items = await queries
                                .Skip(param.Skip)
                                .Take(param.PageSize)
                                .Select(x => new VttCurrencyExchanges
                                {

                                    TransactionsId = x.TransactionsId,
                                    TransactionDate = x.TransactionDate,
                                    TransactionTypeName = x.TransactionTypeName,
                                    BranchName = x.BranchName,
                                    BuyCurrencyName = x.BuyCurrencyName,
                                    BuyAmount = x.BuyAmount,
                                    SellCurrencyName = x.SellCurrencyName,
                                    SellAmount = x.SellAmount,
                                    ExchangeRate = x.ExchangeRate,
                                })
                                .ToListAsync(cancellation);

            return VttCurrencyExchanges.Create(items, totalRecords);
        }
    }
}
