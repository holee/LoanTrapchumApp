using Applications.Features.CashManagements.Responses;
using Applications.Interfaces.Transactions;
using Applications.Responses;
using Dapper;
using Domains.Models.Transaction;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Infrastructures.Repository
{
    public class OpenBranchService(DataContext contextFactory, IDapperConnectionFactory connectionFactory) : IOpenBranchService
    {
        public async Task<ErrorOr.ErrorOr<IEnumerable<VCashVaultBalance>>> GetCashVaultBalance(int? branchRefId, CancellationToken cancellation = default)
        {

            var context = contextFactory;

            var queries = await context.Database
                               .SqlQuery<VCashVaultBalance>($@"
                               SELECT
			                        c.CurrencyId,
                                    c.BranchRefId,
			                        c.CurrencyName,
			                        c.CurrencyCode,
			                        c.VaultBalance AS VaultBalance 
                                FROM VCashVaultBalances c 
                                WHERE BranchRefId={branchRefId}
                                ")
                               .AsNoTracking()
                               .ToListAsync(cancellation);
            return queries;

        }

        public async Task<ErrorOr.ErrorOr<BranchStatus>> OpenBranchAsync(LoanTransaction tranx, CancellationToken cancellation = default)
        {

            try
            {
                var context = contextFactory;

                var transaction = tranx.Adapt<LoanTransaction>();

                transaction.TransactionDate = DateTime.UtcNow;

                context.LoanTransaction.Add(transaction);

                await context.SaveChangesAsync(cancellation);

                return await GetBranchStatus(tranx.UserRefId, cancellation);

            }
            catch (DbUpdateException ex)
            {
                return ErrorOr.Error.Unexpected(ex.Message);
            }
        }


        public async Task<ErrorOr.ErrorOr<BranchStatus>> CloseBranchAsync(LoanTransaction transaction, CancellationToken cancellation = default)
        {

            try
            {
                var context = contextFactory;

                var branchId = transaction.OpenBranch!.BranchRefId!.Value;

                var closeDate = transaction.TransactionDate!.Value;

                if (!(await IsAllreadyVaultIn(branchId, closeDate)))
                {
                    return ErrorOr.Error.NotFound("Cannot Close Branch Now, Please VaultIn First.");
                }

                transaction.TransactionDate = DateTime.UtcNow;

                context.LoanTransaction.Add(transaction);

                await context.SaveChangesAsync();

                return await GetBranchStatus(transaction.UserRefId, cancellation);
            }
            catch (DbUpdateException ex)
            {
                return ErrorOr.Error.Unexpected(ex.Message);
            }


        }



        private async Task<bool> IsAllreadyVaultIn(int brnchId, DateTimeOffset transactionDt)
        {
            //var cashVualt=await context.CashVaults
            //                     .OrderByDescending(x=>x.CVDate)
            //                     .Where(x=>x.BranchRefId==brnchId &&
            //                            x.CVDate==transactionDT &&
            //                            x.CVStatusRefId==7)
            //                     .AsNoTracking()
            //                     .ToListAsync();
            //return cashVualt.Any();
            await Task.CompletedTask;
            return true;
        }





        private async Task<ErrorOr.ErrorOr<BranchStatus>> GetBranchStatus(string? userId, CancellationToken cancellation = default)
        {
            using var context = await connectionFactory.CreateConnectionAsync(cancellation);

            var branch = await context.QueryFirstOrDefaultAsync<BranchStatus>("spGetBranchStatus",
                                        new { UserId = userId },
                                        commandType: CommandType.StoredProcedure);

            if (branch is null)
            {
                return new BranchStatus(0, false, "No branch found for the user.");
            }

            return branch;

        }

    }
}
