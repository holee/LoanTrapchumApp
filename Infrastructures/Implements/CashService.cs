using Applications.Interfaces.Transactions;
using Applications.Responses;
using Applications.Responses.ViewModels.Currencies;
using Domains.Models.Transaction;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Implements
{
    public class CashService:ICashServiceImpl
    {
        private readonly ApplicationDbContext _context;

        public CashService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<CashStatus>> GetsCashStatus(string? q=null)  
        {
            IQueryable<CashStatus> queries = _context.CashStatuses;

            if(!string.IsNullOrEmpty(q))
            {
                queries=queries.Where(x=>EF.Functions.Like(x.Code, $"%{q}%") || EF.Functions.Like(x.Description,$"%{q}%"));
            }

            return await queries.AsNoTracking().ToListAsync();

        }

        public async Task<CashStatusResult> CheckCashValueIsInOut(int branchRefId,DateTime bussinessDate) 
        {
            var query = await _context.Database.SqlQuery<VCashStatus>($"SELECT TOP(1) * FROM VCashStatuses ORDER BY BusinessDate DESC")
                                               .Where(x => x.BranchRefId == branchRefId
                                                                    &&
                                                                    x.BusinessDate == bussinessDate
                                                                    &&
                                                                    x.CashStatusRefId == 8)
                                                .FirstOrDefaultAsync();

            if(query is null)
            {
                return new CashStatusResult
                {
                    IsOut=true,
                    Message="VaultOut"
                };
            }

            return new CashStatusResult
            {
                IsOut = false,
                Message="VaultIn"
            };
        }

    }
}
