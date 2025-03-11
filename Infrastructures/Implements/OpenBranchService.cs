using Applications.Interfaces.Transactions;
using Applications.Responses;
using Applications.Responses.ViewModels.Currencies;
using Domains.Models.Transaction;
using Infrastructures.Data;
using Microsoft.EntityFrameworkCore;

namespace CoreBussiness.Implements
{
    public class OpenBranchService(ApplicationDbContext context) : IOpenBranchServiceImpl
    {


        public async Task<Result<IEnumerable<VCashVaultBalance>>> GetCashVaultBalance(int branchRefId)
        {
            var queries = await context.Database
                            .SqlQuery<VCashVaultBalance>($"SELECT * FROM VCashVaultBalances WHERE BranchRefId={branchRefId}")
                            .AsNoTracking()
                            .ToListAsync();

            if (!queries.Any())
            {
                queries = await context.Database
                               .SqlQuery<VCashVaultBalance>($@"SELECT DISTINCT
			                        c.CurrencyId,
                                    c.BranchRefId,
			                        c.CurrencyName,
			                        c.CurrencyCode,
			                        CAST(0 AS DECIMAL) AS VaultBalance FROM VCashVaultBalances c")
                               .AsNoTracking()
                               .ToListAsync();
                return Result.Success<IEnumerable<VCashVaultBalance>>(queries);
            }

            return Result.Success<IEnumerable<VCashVaultBalance>>(queries);
        }

        public async Task<Result<string>> OpenBranchAsync(LoanTransaction transaction)
        {

            try
            {
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
