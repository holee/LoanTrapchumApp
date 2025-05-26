using Applications.Extensions;
using Applications.Interfaces.Loans;
using Applications.Responses.Auths;
using Applications.Responses.ViewModels.Auths;
using Domains.Models.Loans;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Implements
{
    public sealed class LoanSettingService : ILoanSettingService
    {
        private readonly DataContext _context;
        private readonly IDapperConnectionFactory _db;
        private readonly IDbContextFactory<DataContext> _contextFactory;
        public LoanSettingService(DataContext context, IDapperConnectionFactory db, IDbContextFactory<DataContext> contextFactory)
        {
            _context = context;
            _db = db;
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<LoanSubTypeStatus>> GetSubSubTypeStatus(int? subTypeRefId = null, string? q = null)
        {

            await using var context = _contextFactory.CreateDbContext();

            IQueryable<LoanSubTypeStatus> queries = context.LoanSubTypeNoAcc;

            if (subTypeRefId.HasValue && subTypeRefId > 0)
            {
                queries = queries.Where(x => x.LoanTypeStatusRefId == subTypeRefId.Value);
            }
            if (!string.IsNullOrEmpty(q))
            {
                queries = queries.Where(x => x.Description != null && x.Description.Contains(q));
            }

            return await queries.AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<LoanTypeStatus>> GetSubTypeStatus(string? q = null)
        {
            await using var context = _contextFactory.CreateDbContext();

            var queries = await context.LoanSubTypeStatuses
                                                        .WhereIf(!string.IsNullOrEmpty(q), x => x.Description!.Contains(q))
                                                        .AsNoTracking()
                                                        .ToListAsync();
            return queries;
        }
        public async Task<IEnumerable<LoanRepaymentMode>> GetRepaymentModes(string? q = null)
        {

            await using var context = _contextFactory.CreateDbContext();

            var query = await context.LoanRepaymentModes
                                                .WhereIf(!string.IsNullOrEmpty(q), x => x.RepaymentModeCode!.Contains(q) ||
                                                                                         x.Description!.Contains(q))
                                                .AsNoTracking()
                                                .ToListAsync();
            return query;
        }
        public async Task<IEnumerable<LoanInstallmentType>> GetInstallmentTypes(string? q = null)
        {
            await using var context = _contextFactory.CreateDbContext();

            IQueryable<LoanInstallmentType> query = context.LoanInstallmentTypes;

            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(x => x.InstallmentTypeCode != null && x.InstallmentTypeCode.Contains(q) ||
                                  x.Description != null && x.Description.Contains(q));
            }

            return await query.AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<LoanCategory>> GetLoanCategories(string? q = null)
        {
            await using var context = _contextFactory.CreateDbContext();

            IQueryable<LoanCategory> query = context.LoanCategories.Where(x => x.IsActive == true);

            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(x => x.LoanCategoryCode != null && x.LoanCategoryCode.Contains(q) ||
                                  x.Code != null && x.Code.Contains(q));
            }

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<AprovePerson>> GetAprovedUserAsync(int? branchId, string? filter = null)
        {
            await using var context = _contextFactory.CreateDbContext();

            IQueryable<VUser> queries = context.Database
                                                .SqlQuery<VUser>($"SELECT * FROM vUsers WHERE IsActive=CAST(1 AS BIT)")
                                                .WhereIf(branchId > 0, x => x.BranchId == branchId);

            if (!string.IsNullOrEmpty(filter))
            {
                queries = queries.Where(x => EF.Functions.Like(x.UserName!, $"%{filter}%") ||
                                             EF.Functions.Like(x.FirstName!, $"%{filter}%") ||
                                             EF.Functions.Like(x.LastName!, $"%{filter}%"));
            }

            var aprovedPersons = await queries
                                              .ProjectToType<AprovePerson>()
                                              .AsNoTracking()
                                              .ToListAsync();

            return aprovedPersons;

        }

        public async Task<IEnumerable<T>> GetAprovedUserAsync<T>(int? branchId, string? filter = null)
            where T : class
        {

            await using var context = _contextFactory.CreateDbContext();

            var aprovedPersons = await context.Database
                                                .SqlQuery<VUser>($"SELECT * FROM vUsers WHERE IsActive=CAST(1 AS BIT) AND CanAppRoved=CAST(1 AS BIT)")
                                                .WhereIf(branchId > 0, x => x.BranchId == branchId)
                                                .WhereIf(!string.IsNullOrEmpty(filter), x => EF.Functions.Like(x.UserName!, $"%{filter}%") ||
                                                                                         EF.Functions.Like(x.FirstName!, $"%{filter}%") ||
                                                                                         EF.Functions.Like(x.LastName!, $"%{filter}%"))
                                               .ProjectToType<T>()
                                               .AsNoTracking()
                                               .ToListAsync() ;

            return aprovedPersons;

        }

    }

}
