using Applications.Features.CashManagements.Responses;
using Applications.Interfaces.Transactions;
using Applications.Responses;
using Domains.Models.Transaction;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Implements
{
    public class OpenBranchService(IDbContextFactory<DataContext> contextFactory, IDapperConnectionFactory connectionFactory) : IOpenBranchService
    {
        public async Task<Result<IEnumerable<VCashVaultBalance>>> GetCashVaultBalance(int? branchRefId)
        {

            using var context = await contextFactory.CreateDbContextAsync();

            var    queries = await context.Database
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
                               .ToListAsync();
                return Result.Success<IEnumerable<VCashVaultBalance>>(queries);

        }

        public async Task<Result<string>> OpenBranchAsync(LoanTransaction tranx)
        {

            try
            {
                await using var context = await contextFactory.CreateDbContextAsync();

                var transaction = tranx.Adapt<LoanTransaction>();

                transaction.TransactionDate = DateTime.UtcNow;

                context.LoanTransaction.Add(transaction);

                await context.SaveChangesAsync();

                return Result.Success(new string("Branch Open Successfully."));

            }
            catch (DbUpdateException ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError(ex.Message));
            }
        }


        public async Task<Result<string>> CloseBranchAsync(LoanTransaction transaction)
        {

            try
            {
                await using var context = await contextFactory.CreateDbContextAsync();

                var branchId = transaction.OpenBranch!.BranchRefId!.Value;

                var closeDate = transaction.TransactionDate!.Value;

                if (!(await IsAllreadyVaultIn(branchId, closeDate)))
                {
                    return Result.Failure<string>(ErrorResponse.NotFound("Cannot Close Branch Now, Please VaultIn First."));
                }

                transaction.TransactionDate = DateTime.UtcNow;

                context.LoanTransaction.Add(transaction);

                await context.SaveChangesAsync();

                return Result.Success(new string("Branch Close Successfully"));
            }
            catch (DbUpdateException ex)
            {
                return Result.Failure<string>(ErrorResponse.ServerError(ex.Message));
            }


        }



        private async Task<bool> IsAllreadyVaultIn(int brnchId, DateTime transactionDt)
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


    }
}
