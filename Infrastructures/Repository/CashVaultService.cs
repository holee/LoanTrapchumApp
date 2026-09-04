using Applications.Enumerations;
using Applications.Extensions;
using Applications.Features.CashManagements.Responses;
using Applications.Features.Currencies.Responses;
using Applications.Interfaces.Transactions;
using Applications.Responses;
using Applications.Responses.Message;
using Applications.Responses.Paginations;
using Domains.Models.Transaction;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Infrastructures.Repository
{
    public sealed class CashVaultService : ICashVaultService
    {
        private readonly DataContext _context;

        private readonly IDapperConnectionFactory _dapperFactory;

        public CashVaultService(DataContext context, IDapperConnectionFactory connectionFactory)
        {
            _context = context;
            _dapperFactory = connectionFactory;
        }

        public async Task<Result> CreateCashManagementAsync(LoanTransaction cashTransaction, CancellationToken cancellation = default)
        {
            try
            {

                var context = _context;

                context.LoanTransaction.Add(cashTransaction);

                await context.SaveChangesAsync(cancellation);

                return Result.Success("Cash vault transaction succeed");
            }
            catch (Exception ex)
            {
                return Result.Failure(ErrorResponse.ServerError($"{ex.Message}"));
            }

        }


        public async Task<Result> CreateCashVaultAsync(LoanTransaction trnx, CancellationToken cancellation = default)
        {
            var context = _context;

            var cashStatus = trnx.CashManagements.First();

            if ((cashStatus.CashStatusRefId == (int)CashStatusEnum.VAULTIN) &&
                    await IsAlreadyVaultIn(context, cashStatus.BranchRefId, trnx.TransactionDate))
            {
                return Result.Failure(ErrorResponse.Validation("Selected Branch is already vault in."));
            }

            var strategy = context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
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
            });
        }




        public async Task<ErrorOr.ErrorOr<string>> CashVaultAsync(LoanTransaction trnx, CancellationToken cancellation = default)
        {
            var context = _context;

            if (await IsAlreadyVaultIn(context, trnx.BranchRefId, trnx.TransactionDate))
            {
                return ErrorOr.Error.Validation("Selected Branch is already vault in.");
            }

            var strategy = context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
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

                    return "Cash vault transaction succeed";
                }
                catch (Exception ex)
                {
                    await dbTransaction.RollbackAsync(cancellation);
                    return (ErrorOr.ErrorOr<string>)ErrorOr.Error.Unexpected($"Failed to create cash vault: {ex.Message}");
                }
            });
        }



        public async Task<IEnumerable<VCurrencyNotes>> GetCurrencyNotesAsync(int? branchId, CancellationToken cancellation = default)
        {

            var context = _context;

            var items = await context.Database
                               .SqlQuery<VCurrencyNotes>($@"SELECT * FROM VCurrencyNotes WHERE BranchId={branchId}")
                               .AsNoTracking()
                               .ToListAsync();
            return items ?? [];

        }

        public async Task<IEnumerable<VCashVaults>> GetViewCashVaultAsync(CancellationToken cancellation = default)
        {

            var context = _context;

            var results = context.Database.SqlQuery<VCashVaults>($@"SELECT * FROM VCashVaults");

            var vaults = await results.AsNoTracking().ToListAsync(cancellation);

            return vaults;
        }


        public async Task<Pager<VCashVaults>> GetViewCashVaultAsync(int? branchId, CancellationToken cancellation = default)
        {

            var context = _context;

            var vaults = context.Set<VCashVaults>()
                                .Where(x => x.BranchRefId == branchId)
                                .AsNoTracking();

            var counts = await vaults.CountAsync(cancellation);

            var items = await vaults.AsNoTracking().ToListAsync(cancellation);

            return new Pager<VCashVaults>(items, counts);
        }

        public async Task<VCashVaults?> GetCashVaultAsync(int? currencyId, int? branchId, CancellationToken cancellation = default)
        {

            var context = _context;


            var vaults = await context.Database.SqlQuery<VCashVaults>($@"SELECT * FROM VCashVaults WHERE CurrencyRefId={currencyId} AND BranchRefId={branchId}")
                                        .AsNoTracking().FirstOrDefaultAsync(cancellation);
            return vaults;

        }

        public async Task<Result<IEnumerable<VCashVaults>>> GetCashVaultsAsync(int? branchId, CancellationToken cancellation = default)
        {

            var context = _context;


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


        private async Task<bool> IsAlreadyVaultIn(DataContext context, int? branchId, DateTimeOffset? trnxDate)
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


//public async Task<Result> CreateCashVaultAsync(LoanTransaction transaction, CancellationToken cancellation = default)
//{
//    var context = _context;

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

