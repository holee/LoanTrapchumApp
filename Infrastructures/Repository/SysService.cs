using Applications.Interfaces.setting;
using Domains.Models.Collaterals;
using Domains.Models.Loans;
using Domains.Models.Transaction;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repository
{
    public class SysService:ISysService
    {
        private readonly DataContext _context;

        public SysService(DataContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<CashStatus>> GetCashStatusAsync(string? q = null)
        {
            IQueryable<CashStatus> queries = _context.CashStatuses;

            if (!string.IsNullOrEmpty(q))
            {
                queries = queries.Where(x => x.Code != null && x.Code.Contains(q)
                                        ||
                                        x.CashStatusId.ToString().Equals(q));
            }

            return await queries.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<CollateralType>> GetCollateralTypes(string? q = null)
        {
            IQueryable<CollateralType> queries = _context.CollateralTypes;

            if (!string.IsNullOrEmpty(q))
            {
                queries = queries.Where(x => x.CollateralTypeCode != null && x.CollateralTypeCode.Contains(q)
                                        ||
                                        x.CollateralTypeId.ToString().Equals(q));
            }

            return await queries.AsNoTracking().ToListAsync();
        }

        
        public async Task<IEnumerable<LoanScheduleStatus>> GetLoanScheduleStatuses(string? q = null)
        {
            IQueryable<LoanScheduleStatus> query = _context.LoanScheduleStatuses;

            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(x => x.LoanScheduleStatusCode != null && x.LoanScheduleStatusCode.Contains(q) ||
                                  x.Description != null && x.Description.Contains(q));
            }

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<LoanStatus>> GetLoanStatuses(string? q = null)
        {
            IQueryable<LoanStatus> query = _context.LoanStatuses;

            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(x => x.Loanstatuscode != null && x.Loanstatuscode.Contains(q) ||
                                  x.Description != null && x.Description.Contains(q));
            }

            return await query.AsNoTracking().ToListAsync();
        }

        

        public async Task<IEnumerable<LoanTermType>> GetLoanTerms(string? q = null)
        {
            IQueryable<LoanTermType> query = _context.LoanTerms;

            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(x => x.LoanTermCode != null && x.LoanTermCode.Contains(q) ||
                                  x.Description != null && x.Description.Contains(q));
            }

            return await query.AsNoTracking().ToListAsync();

        }

        public async Task<IEnumerable<LoanPaymentLocation>> GetPaymentLocations(string? q = null)
        {
            IQueryable<LoanPaymentLocation> query = _context.LoanPaymentLocations;

            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(x => x.Location != null && x.Location.Contains(q));
            }

            return await query.AsNoTracking().ToListAsync();
        }
    }
}
