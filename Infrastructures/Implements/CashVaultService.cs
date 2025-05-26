using Applications.Interfaces.Transactions;
using Applications.Responses;
using Applications.Responses.Message;
using Applications.Responses.ViewModels.Currencies;
using Dapper;
using Domains.Models.Transaction;
using Infrastructures.Constants.Procedures;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Infrastructures.Implements
{
    public class CashVaultService(DataContext context, IDapperConnectionFactory connectionFactory) : ICashVaultService
    {
        public async Task<Result> CreateCashManagementAsync(LoanTransaction cashTransaction)
        {
            try
            {
                context.LoanTransaction.Add(cashTransaction);
                await context.SaveChangesAsync();
                return Result.Success("Cash vault transaction succeed");
            }
            catch (Exception ex)
            {
                return Result.Failure(ErrorResponse.ServerError($"{ex.Message}"));
            }

        }

        public async Task<Result> CreateCashVaultAsync(LoanTransaction transaction)
        {
            try
            {
                await context.Database.BeginTransactionAsync();
                context.LoanTransaction.Add(transaction);
                await context.SaveChangesAsync();
                var cash = transaction?.CashManagements?.FirstOrDefault();
                cash?.CashManagementDetails?.ForEach(x =>
                {
                    x.TransactionRefId = transaction!.TransactionsId;
                });
                context.Update(transaction!);
                await context.SaveChangesAsync();
                await context.Database.CommitTransactionAsync();
                return Result.Success("Cash vault transaction succeed");
            }
            catch (Exception ex)
            {
                context.Database.RollbackTransaction();
                return Result.Failure(ErrorResponse.ServerError($"{ex.Message}"));
            }
        }

        public async Task<IEnumerable<ViewModelCurrencyNotes>> GetCurrencyNotesAsync(int? branchId)
        {

            using var db= await connectionFactory.CreateConnectionAsync();

            var queries = await db.QueryAsync<ViewModelCurrencyNotes>(ProcedureConstant.GetCurrencyNotesByBranchId, new { branchId }, commandType: CommandType.StoredProcedure);

            return queries;
        }

        public async Task<IEnumerable<VCashVaults>> GetViewCashVaultAsync()
        {
            IQueryable<VCashVaults> results = context.Database.SqlQuery<VCashVaults>($@"SELECT * FROM VCashVaults");

            var vaults = await results.AsNoTracking()
                                        .ToListAsync();

            return vaults;
        }


        public async Task<VCashVaults?> GetCashVaultAsync(int? currencyId, int? branchId)
        {
            var vaults = await context.Database.SqlQuery<VCashVaults>($@"SELECT * FROM VCashVaults WHERE CurrencyRefId={currencyId} AND BranchRefId={branchId}")
                                        .AsNoTracking().FirstOrDefaultAsync();
            return vaults;

        }

        public async Task<Result<IEnumerable<VCashVaults>>> GetCashVaultsAsync(int? branchId)
        {
            var results = await context.Database
                                                 .SqlQuery<VCashVaults>($@"SELECT * FROM VCashVaults")
                                                 .Where(x => x.BankRefId == branchId)
                                                 .AsNoTracking()
                                                 .ToListAsync();
            if (!results.Any())
            {
                return Result.Failure<IEnumerable<VCashVaults>>(ErrorResponse.NotFound(Messages.DataNotFound));
            }

            return Result.Success<IEnumerable<VCashVaults>>(results);
        }
    }
}
