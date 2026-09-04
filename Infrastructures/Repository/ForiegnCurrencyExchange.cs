using Applications.Exceptions;
using Applications.Features.Currencies.Responses;
using Applications.Interfaces.Transactions;
using Applications.Responses;
using Applications.Responses.Paginations;
using Domains.Models.Transaction;
using Infrastructures.DataAccess;
using Infrastructures.Repository.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repository
{
    public sealed class ForiegnCurrencyExchange : IForiegnCurrencyExchange
    {
        private readonly DataContext _contextFactory;
        public ForiegnCurrencyExchange(DataContext contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Result<string>> Exchange(LoanTransaction transaction)
        {
            var context = _contextFactory;

            try
            {
                context.LoanTransaction.Add(transaction);

                await context.SaveChangesAsync();

                return Result.Success(new string("Exchange Success."));
            }
            catch (Exception ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError($"{ex.Message}"));
            }
        }


        public async Task<ErrorOr.ErrorOr<string>> CreateExchangeAsync(LoanTransaction transaction, CancellationToken cancellation = default)
        {
            var context = _contextFactory;

            try
            {
                context.LoanTransaction.Add(transaction);

                await context.SaveChangesAsync(cancellation);

                return transaction.TransactionsId;
            }
            catch (DbUpdateException ex)
            {
                return ErrorOr.Error.Failure("Error", ex.Message);
            }
        }

        public async Task<ErrorOr.ErrorOr<VttCurrencyExchanges>> CreateExchangeAsync(ForeignExchange transaction, CancellationToken cancellation = default)
        {
            var context = _contextFactory;

            try
            {
                context.ForeignExchanges.Add(transaction);

                await context.SaveChangesAsync(cancellation);

                var result = await GetCurrencyExchange(context, transaction.BranchRefId, transaction.TransactionRefId, cancellation);

                return result;
            }
            catch (DbUpdateException ex)
            {
                return ErrorOr.Error.Failure("Error", ex.Message);
            }
        }


        public async Task<ErrorOr.ErrorOr<VttCurrencyExchanges>> EditExchangeAsync(ForeignExchange transaction, CancellationToken cancellation = default)
        {

            var context = _contextFactory;

            try
            {
                context.ForeignExchanges.Update(transaction);

                await context.SaveChangesAsync(cancellation);

                var result = await GetCurrencyExchange(context, transaction.BranchRefId, transaction.TransactionRefId, cancellation);

                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return ErrorOr.Error.Failure("Error", ex.Message);
            }
        }


        public async Task<VttCurrencyExchanges> GetCurrencyExchange(int? branchId, string? transactionId, DateTimeOffset? transactionDate, CancellationToken cancellation = default)
        {

            var context = _contextFactory;

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
            var context = _contextFactory;

            var queries = context.Set<VttCurrencyExchanges>()
                                .Where(x => x.BranchId == branchId)
                                .OrderByDescending(x => x.TransactionDate)
                                .AsNoTracking();

            if (transactionDate.HasValue)
            {
                var date = transactionDate.Value.Date;

                queries = queries.Where(x => x.TransactionDate.HasValue && x.TransactionDate.Value.Date == date);

            }

            queries = queries.ApplyOrdering(param);

            var totalRecords = await queries.CountAsync(cancellation);

            var items = await queries
                                .Skip(param.Page)
                                .Take(param.Take)
                                .Select(x => new VttCurrencyExchanges
                                {

                                    TransactionsId = x.TransactionsId,
                                    TransactionDate = x.TransactionDate,
                                    TransactionTypeName = x.TransactionTypeName,
                                    BranchName = x.BranchName,
                                    BuyCurrencyName = x.BuyCurrencyName,
                                    BuyAmount = x.BuyAmount,
                                    BuyCurrencyRefId = x.BuyCurrencyRefId,
                                    SellCurrencyName = x.SellCurrencyName,
                                    SellAmount = x.SellAmount,
                                    SellCurrencyRefId = x.SellCurrencyRefId,
                                    ExchangeRate = x.ExchangeRate,
                                    IsBuy = x.IsBuy,
                                })
                                .ToListAsync(cancellation);

            return VttCurrencyExchanges.Create(items, totalRecords);
        }


        private async Task<VttCurrencyExchanges> GetCurrencyExchange(DataContext context, int? branchId, string? transactionId, CancellationToken cancellation = default)
        {

            var result = await context.Database
                                .SqlQuery<VttCurrencyExchanges>($"SELECT * FROM VTTExchangeTransactions WHERE branchId={branchId} AND transactionsId={transactionId}")
                                .AsNoTracking()
                                .FirstOrDefaultAsync(cancellation);

            if (result is null)
            {
                throw new CurrencyNotFoundException();
            }

            return result;
        }
    }
}
