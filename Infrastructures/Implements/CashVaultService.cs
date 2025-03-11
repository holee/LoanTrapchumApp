using Applications.Interfaces.Transactions;
using Applications.Responses;
using Applications.Responses.ViewModels.Currencies;
using Domains.Models.Transaction;
using Infrastructures.Data;
using Microsoft.EntityFrameworkCore;

namespace CoreBussiness.Implements
{
    public class CashVaultService : ICashVaultServiceImpl
    {
        private readonly ApplicationDbContext _context;

        public CashVaultService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<Result> CreateCashManagementAsync(LoanTransaction cashTransaction)
        {
            try
            {
                _context.LoanTransaction.Add(cashTransaction);
                await _context.SaveChangesAsync();
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
                await _context.Database.BeginTransactionAsync();
                _context.LoanTransaction.Add(transaction);
                await _context.SaveChangesAsync();
                var cash = transaction?.CashManagements?.FirstOrDefault();
                cash?.CashManagementDetails?.ForEach(x =>
                {
                    x.TranRefId = transaction!.TranId;
                });
                _context.Update(transaction!);
                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();
                return Result.Success("Cash vault transaction succeed");
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                return Result.Failure(ErrorResponse.ServerError($"{ex.Message}"));
            }
        }

        public async Task<IEnumerable<ViewModelCurrencyNotes>> GetCurrencyNotesAsync(int branchId)
        {
            IQueryable<ViewModelCurrencyNotes> results = _context.Database.SqlQuery<ViewModelCurrencyNotes>($@"SELECT * FROM fGetCurrencyNotesByBranch({branchId})");
            var notes = await results.Where(x => x.IsActive == true)
                                        .AsNoTracking()
                                        .OrderBy(x => x.CurrencyValue)
                                        .ToListAsync();
            return notes;
        }

        public async Task<IEnumerable<VCashVaults>> GetViewCashVaultAsync()
        {
            IQueryable<VCashVaults> results = _context.Database.SqlQuery<VCashVaults>($@"SELECT * FROM VCashVaults");

            var vaults = await results.AsNoTracking()
                                        .ToListAsync();

            return vaults;
        }


        public async Task<VCashVaults?> GetCashVaultAsync(int? currencyId, int? branchId)
        {
            var vaults = await _context.Database.SqlQuery<VCashVaults>($@"SELECT * FROM VCashVaults WHERE CurrencyRefId={currencyId} AND BranchRefId={branchId}")
                                        .AsNoTracking().FirstOrDefaultAsync();
            return vaults;

        }


    }
}
