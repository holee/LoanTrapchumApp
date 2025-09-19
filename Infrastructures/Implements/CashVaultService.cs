using Applications.Enumerations;
using Applications.Features.CashManagements.Responses;
using Applications.Features.Currencies.Responses;
using Applications.Interfaces.Transactions;
using Applications.Responses;
using Applications.Responses.Message;
using Domains.Models.Transaction;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Infrastructures.Implements
{
    public sealed class CashVaultService : ICashVaultService
    {
        private readonly IDbContextFactory<DataContext> _context;

        private readonly IDapperConnectionFactory _dapperFactory;

        public CashVaultService(IDbContextFactory<DataContext> context, IDapperConnectionFactory connectionFactory)
        {
            _context = context;
            _dapperFactory = connectionFactory;
        }

        public async Task<Result> CreateCashManagementAsync(LoanTransaction cashTransaction, CancellationToken cancellation = default)
        {
            try
            {

                await using var context = await _context.CreateDbContextAsync(cancellation);

                context.LoanTransaction.Add(cashTransaction);

                await context.SaveChangesAsync(cancellation);

                return Result.Success("Cash vault transaction succeed");
            }
            catch (Exception ex)
            {
                return Result.Failure(ErrorResponse.ServerError($"{ex.Message}"));
            }

        }

        //public async Task<Result> CreateCashVaultAsync(LoanTransaction transaction, CancellationToken cancellation = default)
        //{
        //    await using var context = await _context.CreateDbContextAsync(cancellation);

        //    try
        //    {
        //        await context.Database.BeginTransactionAsync(cancellation);

        //        context.LoanTransaction.Add(transaction);

        //        await context.SaveChangesAsync(cancellation);

        //        var cash = transaction?.CashManagements?.FirstOrDefault();

        //        cash?.CashManagementDetails?.ForEach(x =>
        //        {
        //            x.TransactionRefId = transaction!.TransactionsId;
        //        });

        //        context.Update(transaction!);

        //        await context.SaveChangesAsync(cancellation);

        //        await context.Database.CommitTransactionAsync();

        //        return Result.Success("Cash vault transaction succeed");
        //    }
        //    catch (Exception ex)
        //    {
        //        context.Database.RollbackTransaction();

        //        return Result.Failure(ErrorResponse.ServerError($"{ex.Message}"));
        //    }
        //}


        public async Task<Result> CreateCashVaultAsync(LoanTransaction trnx, CancellationToken cancellation = default)
        {
            await using var context = await _context.CreateDbContextAsync(cancellation);

            var cashStatus = trnx.CashManagements.First();

            if ((cashStatus.CashStatusRefId == (int)CashStatusEnum.VAULTIN) &&
                    await IsAlreadyVaultIn(context, cashStatus.BranchRefId, trnx.TransactionDate))
            {
                return Result.Failure(ErrorResponse.Validation("Selected Branch is already vault in."));
            }

            await using var dbTransaction = await context.Database.BeginTransactionAsync(cancellation);

            try
            {

                // Set FK values before saving
                if (trnx?.CashManagements is not null)
                {
                    foreach (var cash in trnx.CashManagements)
                    {
                        cash.TransactionRefId = trnx.TransactionsId;

                        if (cash.CashManagementDetails!.Any())
                        {
                            foreach (var detail in cash.CashManagementDetails!)
                            {
                                detail.TransactionsRefId = trnx.TransactionsId;

                                detail.CachmanagementRefId = cash.Id;
                            }
                        }
                    }
                }

                // Add root entity (cascades children because of lists)
                await context.LoanTransaction.AddAsync(trnx!, cancellation);

                await context.SaveChangesAsync(cancellation);

                await dbTransaction.CommitAsync(cancellation);

                return Result.Success("Cash vault transaction succeed");
            }
            catch (Exception ex)
            {
                await dbTransaction.RollbackAsync(cancellation);

                return Result.Failure(ErrorResponse.ServerError($"Failed to create cash vault: {ex.Message}"));
            }
        }



        public async Task<IEnumerable<VCurrencyNotes>> GetCurrencyNotesAsync(int? branchId, CancellationToken cancellation = default)
        {

            await using var context = await _context.CreateDbContextAsync(cancellation);

            var items = await context.Database
                               .SqlQuery<VCurrencyNotes>($@"SELECT * FROM VCurrencyNotes WHERE BranchId={branchId}")
                               .AsNoTracking()
                               .ToListAsync();
            return items ?? [];

            //using var db = await _dapperFactory.CreateConnectionAsync(cancellation);

            //var queries = await db.QueryAsync<VCurrencyNotes>(ProcedureConstant.GetCurrencyNotesByBranchId, new { branchId }, commandType: CommandType.StoredProcedure);

            //return queries;
        }

        public async Task<IEnumerable<VCashVaults>> GetViewCashVaultAsync(CancellationToken cancellation = default)
        {

            await using var context = await _context.CreateDbContextAsync(cancellation);


            var results = context.Database.SqlQuery<VCashVaults>($@"SELECT * FROM VCashVaults");

            var vaults = await results.AsNoTracking().ToListAsync(cancellation);

            return vaults;
        }


        public async Task<IEnumerable<VCashVaults>> GetViewCashVaultAsync(int? branchId, CancellationToken cancellation = default)
        {

            await using var context = await _context.CreateDbContextAsync(cancellation);

            var results = context.Database.SqlQuery<VCashVaults>($@"SELECT * FROM VCashVaults WHERE BranchRefId={branchId}");

            var vaults = await results.AsNoTracking().ToListAsync(cancellation);

            return vaults;
        }

        public async Task<VCashVaults?> GetCashVaultAsync(int? currencyId, int? branchId, CancellationToken cancellation = default)
        {

            await using var context = await _context.CreateDbContextAsync(cancellation);


            var vaults = await context.Database.SqlQuery<VCashVaults>($@"SELECT * FROM VCashVaults WHERE CurrencyRefId={currencyId} AND BranchRefId={branchId}")
                                        .AsNoTracking().FirstOrDefaultAsync(cancellation);
            return vaults;

        }

        public async Task<Result<IEnumerable<VCashVaults>>> GetCashVaultsAsync(int? branchId, CancellationToken cancellation = default)
        {

            await using var context = await _context.CreateDbContextAsync(cancellation);


            var results = await context.Database
                                       .SqlQuery<VCashVaults>($@"SELECT * FROM VCashVaults WHERE BranchRefId={branchId}")
                                       .AsNoTracking()
                                       .ToListAsync();
            if (!results.Any())
            {
                return Result.Failure<IEnumerable<VCashVaults>>(ErrorResponse.NotFound(Messages.DataNotFound));
            }

            return Result.Success<IEnumerable<VCashVaults>>(results);
        }


        private async Task<bool> IsAlreadyVaultIn(DataContext context, int? branchId, DateTime? trnxDate)
        {
            return await context.LoanTransaction
                .Where(x => x.TransactionDate == trnxDate)
                .AnyAsync(x => x.CashManagements.Any(cm =>
                    cm.BranchRefId == branchId &&
                    cm.CashStatusRefId == (int)CashStatusEnum.VAULTIN &&
                    cm.TransactionDate == trnxDate));
        }




    }
}
