using Applications.Extensions;
using Applications.Features.Settings.Dto;
using Applications.Interfaces.Loans;
using Applications.Responses.Loans.Disbursements;
using Applications.Responses.Paginations;
using Domains.Models.Loans;
using Infrastructures.DataAccess;
using Infrastructures.DataAccess.Paging;
using Infrastructures.Implements.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Implements
{
    public sealed class LoanSettingRepository : ILoanSettingService
    {
        private readonly IDapperConnectionFactory _db;
        private readonly IDbContextFactory<DataContext> _contextFactory;
        private readonly ILogger<LoanSettingRepository> _logger;
        private const int _mainOffice = 1;
        public LoanSettingRepository(IDapperConnectionFactory db,
            IDbContextFactory<DataContext> contextFactory,
            ILogger<LoanSettingRepository> logger)
        {
            _db = db;
            _contextFactory = contextFactory;
            _logger = logger;
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

        public async Task<Pager<LoanCategoryDto>> GetLoanCategories(int? id, QueryParam param, CancellationToken cancellation = default)
        {
            await using var context = await _contextFactory.CreateDbContextAsync(cancellation);

            var baseQueries = context.LoanCategories
                        .Where(x => x.IsActive == true)
                        .AsNoTracking();

            if (!string.IsNullOrEmpty(param.Filter))
            {
                string[] filterColums = [
                            nameof(LoanCategoryDto.Code),
                            nameof(LoanCategoryDto.LoanCategoryCode)
                    ];

                baseQueries = baseQueries.AndWhereEndWith(param.Filter, filterColums);
            }

            baseQueries = baseQueries.WhereIf(id > 0, x => x.LoanCategoryId == id);

            var counts = await baseQueries.CountAsync(cancellation);

            if (param.Page > 0)
            {
                baseQueries = baseQueries.Skip(param.Page).Take(param.Take);
            }

            var items = await baseQueries
                        .Select(x => new LoanCategoryDto
                        {
                            LoanCategoryId = x.LoanCategoryId,
                            Code = x.Code,
                            LoanCategoryCode = x.LoanCategoryCode,
                            Description = x.Description,
                            DesInKhmer = x.DesInKhmer,
                            IsActive = x.IsActive,
                            EditSeq = x.EditSeq,
                        })
                        .ToListAsync(cancellation);

            return PagerExtensions.Create(items, counts);
        }

        public async Task<IEnumerable<VUserLoanSizes>> GetAprovedUserAsync(int? branchId, int? currencyId,
                decimal? amount, QueryParam param, CancellationToken cancellation = default)
        {
            _logger.LogInformation(
                "GetAprovedUserAsync called with branchId: {BranchId}, currencyId: {CurrencyId}, amount: {Amount}",
                branchId,
                currencyId,
                amount);

            await using var context = await _contextFactory.CreateDbContextAsync(cancellation);

            var baseQuery = context.Set<VUserLoanSizes>()
                                   .WhereIf(branchId.HasValue && branchId > 0,
                                            x => (x.BranchId == branchId || x.BranchId == _mainOffice))
                                   .WhereIf(((amount.HasValue && amount > 0) &&
                                            (currencyId.HasValue && currencyId > 0)),
                                             x => ((currencyId == 1 && amount >= x.MinKhrLoanSize && amount <= x.MaxKhrLoanSize) ||
                                                  (currencyId == 2 && amount >= x.MinUsdLoanSize && amount <= x.MaxUsdLoanSize)) || x.BranchId == _mainOffice);


            if (!string.IsNullOrEmpty(param.Filter))
            {

                string filter = param.Filter.Trim();

                string[] columns = [nameof(VUserLoanSizes.FullName), nameof(VUserLoanSizes.BranchName)];

                baseQuery = baseQuery.WhereMatchesFilter(filter, columns);

            }

            return await baseQuery.AsNoTracking().ToListAsync(cancellation);

        }





        public async Task<Pager<VUserLoanSizes>> GetAprovedUsersAsync(int? branchId, int? currencyId,
             decimal? amount, QueryParam param, CancellationToken cancellation = default)
        {
            _logger.LogInformation(
                "GetAprovedUserAsync called with branchId: {BranchId}, currencyId: {CurrencyId}, amount: {Amount}",
                branchId,
                currencyId,
                amount);

            await using var context = await _contextFactory.CreateDbContextAsync(cancellation);

            var baseQuery = context.Set<VUserLoanSizes>()
                                   .WhereIf(branchId.HasValue && branchId > 0, x => x.BranchId == branchId)
                                   .WhereIf((amount.HasValue && amount > 0) &&
                                             (currencyId.HasValue && currencyId > 0),
                                             x => (currencyId == 1 && amount >= x.MinKhrLoanSize && amount <= x.MaxKhrLoanSize) ||
                                                  (currencyId == 2 && amount >= x.MinUsdLoanSize && amount <= x.MaxUsdLoanSize))
                                   .OrderByDescending(x => x.MaxKhrLoanSize)
                                   .ThenByDescending(x => x.MaxUsdLoanSize)
                                   .AsQueryable();


            if (!string.IsNullOrEmpty(param.Filter))
            {

                string filter = param.Filter.Trim();

                string[] columns = [nameof(VUserLoanSizes.FullName), nameof(VUserLoanSizes.BranchName)];

                baseQuery = baseQuery.WhereMatchesFilter(filter, columns);
            }


            var counts = await baseQuery.CountAsync(cancellation);


            if (param.Take > 0 && param.Page > 0)
            {

                var skip = (param.Page - 1) * param.Take;

                baseQuery = baseQuery.Skip(skip).Take(param.Take);
            }

            var items = await baseQuery.AsNoTracking().ToListAsync(cancellation);

            return PagerExtensions.Create(items, counts);


        }

    }

}

//var items = await context.Database
//    .SqlQuery<VUserLoanSizes>($@"
//            SELECT *
//            FROM VUserLoanSizes
//            WHERE 
//                IsActive = CAST(1 AS BIT)
//                AND ({branchId} IS NULL  OR BranchId = {branchId})
//                AND (
//                    ({currencyId} = 1 AND {amount} BETWEEN MinKhrLoanSize AND MaxKhrLoanSize)
//                    OR
//                    ({currencyId} = 2 AND {amount} BETWEEN MinUsdLoanSize AND MaxUsdLoanSize)
//                ) AND (
//                    {param.Filter} IS NULL 
//                    OR {param.Filter} = N'' 
//                    OR FullName LIKE  {param.Filter} + N'%'
//                    OR BranchName LIKE  {param.Filter} + N'%')
//                   UNION
//            SELECT *  FROM VUserLoanSizes WHERE IsActive = CAST(1 AS BIT) AND CanAppRoved = CAST(1 AS BIT) AND BranchId = 123
//                ").AsNoTracking()
//                .ToListAsync(cancellation);


//public async Task<IEnumerable<LoanCategory>> GetLoanCategories(string? q = null)
//{
//    await using var context = _contextFactory.CreateDbContext();

//    IQueryable<LoanCategory> query = context.LoanCategories.Where(x => x.IsActive == true);

//    if (!string.IsNullOrEmpty(q))
//    {
//        query = query.Where(x => x.LoanCategoryCode != null && x.LoanCategoryCode.Contains(q) ||
//                          x.Code != null && x.Code.Contains(q));
//    }

//    return await query.AsNoTracking().ToListAsync();
//}

//public async Task<IEnumerable<VUserLoanSizes>> GetAprovedUserAsync(int? branchId, int? currencyId, decimal? amount, string? filter = null)
//{
//    _logger.LogInformation(
//        "GetAprovedUserAsync called with branchId: {BranchId}, currencyId: {CurrencyId}, amount: {Amount}",
//        branchId,
//        currencyId,
//        amount);

//    if (amount is null || amount <= 0 || (currencyId is not ((int)CurrenciesType.Khr or (int)CurrenciesType.Usd)))
//    {
//        return [];
//    }

//    using var db = _db.CreateConnection();

//    var parameters = new DynamicParameters();
//    parameters.Add("@branchId", branchId);
//    parameters.Add("@currencyId", currencyId);
//    parameters.Add("@amount", amount);
//    parameters.Add("@filter", string.IsNullOrWhiteSpace(filter) ? null : filter.Trim());

//    var results = await db.QueryAsync<VUserLoanSizes>(@"
//    SELECT 
//        UserRefId,
//        FullName
//    FROM VUserLoanSizes
//    WHERE 
//        IsActive = CAST(1 AS BIT)
//        AND ((@branchId > 0 AND BranchId = @branchId) OR @branchId IS NULL)
//        AND (
//            (@currencyId = 1 AND @amount BETWEEN MinKhrLoanSize AND MaxKhrLoanSize)
//            OR 
//            (@currencyId = 2 AND @amount BETWEEN MinUsdLoanSize AND MaxUsdLoanSize)
//        )
//        AND (
//            @filter IS NULL OR @filter = N'' 
//            OR FullName LIKE N'%' + @filter + N'%' 
//            OR BranchName LIKE N'%' + @filter + N'%'
//        );", parameters);
//    return results;
//}


//public async Task<IEnumerable<AprovePerson>> GetAprovedUserAsync(int? branchId, string? filter = null)
//{
//    await using var context = _contextFactory.CreateDbContext();

//    IQueryable<VUser> queries = context.Database
//                                        .SqlQuery<VUser>($"SELECT * FROM vUsers WHERE IsActive=CAST(1 AS BIT)")
//                                        .WhereIf(branchId > 0, x => x.BranchId == branchId);

//    if (!string.IsNullOrEmpty(filter))
//    {
//        queries = queries.Where(x => EF.Functions.Like(x.UserName!, $"%{filter}%") ||
//                                     EF.Functions.Like(x.FirstName!, $"%{filter}%") ||
//                                     EF.Functions.Like(x.LastName!, $"%{filter}%"));
//    }

//    var aprovedPersons = await queries
//                                      .ProjectToType<AprovePerson>()
//                                      .AsNoTracking()
//                                      .ToListAsync();

//    return aprovedPersons;

//}

//public async Task<IEnumerable<T>> GetAprovedUserAsync<T>(int? branchId, string? filter = null)
//    where T : class
//{

//    await using var context = _contextFactory.CreateDbContext();

//    var aprovedPersons = await context.Database
//                                        .SqlQuery<VUser>($"SELECT * FROM vUsers WHERE IsActive=CAST(1 AS BIT) AND CanAppRoved=CAST(1 AS BIT)")
//                                        .WhereIf(branchId > 0, x => x.BranchId == branchId)
//                                        .WhereIf(!string.IsNullOrEmpty(filter), x => EF.Functions.Like(x.UserName!, $"%{filter}%") ||
//                                                                                 EF.Functions.Like(x.FirstName!, $"%{filter}%") ||
//                                                                                 EF.Functions.Like(x.LastName!, $"%{filter}%"))
//                                       .ProjectToType<T>()
//                                       .AsNoTracking()
//                                       .ToListAsync();

//    return aprovedPersons;

//}
