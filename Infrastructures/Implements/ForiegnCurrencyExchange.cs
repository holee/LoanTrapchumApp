using Applications.Interfaces.Transactions;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Currencies;
using Domains.Models.Transaction;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Implements
{
    public sealed class ForiegnCurrencyExchange : IForiegnCurrencyExchange
    {
        private readonly DataContext _context;

        public ForiegnCurrencyExchange(DataContext context)
        {
            this._context = context;
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


        public async Task<Pager<VttCurrencyExchanges>> GetCurrencyExchange(int branchId, int pageIndex, int pageSize, DateTime? transactionDate = null)
        {

            var queries = _context.Database.SqlQuery<VttCurrencyExchanges>($"SELECT * FROM VTTExchangeTransactions WHERE branchId={branchId}");

            if (transactionDate is not null && transactionDate.HasValue)
            {
                queries = queries.Where(t => t.TransactionDate == transactionDate);
            }

            if (!queries.Any())
            {
                return new Pager<VttCurrencyExchanges>([], 0);
            }

            int counts = await queries.CountAsync();
            var items = await queries.ToListAsync();
            return new Pager<VttCurrencyExchanges>(items, counts);


        }


    }
}
