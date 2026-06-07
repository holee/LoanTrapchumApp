using Applications.Extensions;
using Applications.Features.Collaterals.Dtos;
using Applications.Features.Settings.Dto;
using Applications.Interfaces.Settings;
using Applications.Responses;
using Applications.Responses.Paginations;
using Domains.Models.Collaterals;
using Domains.Models.Loans;
using Domains.Models.Settings;
using ErrorOr;
using Infrastructures.DataAccess;
using Infrastructures.DataAccess.Paging;
using Infrastructures.Implements.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Implements;

public sealed class AppSettingService(IDbContextFactory<DataContext> dbContextFactory, ILogger<AppSettingService> logger) : IAppSettingService
{

    public async Task<ErrorOr<IEnumerable<AppSettingDto>>> GetAppSettingsAsync(string? filter = null, CancellationToken cancellation = default)
    {
        try
        {
            await using var cxt = await dbContextFactory.CreateDbContextAsync(cancellation);

            var query = cxt.AppSettings.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filter))
                query = query.Where(x =>
                    x.Description!.Contains(filter) ||
                    x.Code.Contains(filter) ||
                    x.Value.Contains(filter));

            var items = await query.Select(x => new AppSettingDto
            {
                Id = x.Id,
                Code = x.Code,
                Value = x.Value,
                Description = x.Description,
                IsActive = x.IsActive
            }).ToListAsync(cancellation);

            return items;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Error.Failure(description: ex.Message);
        }
    }

    public async Task<Pager<AppSettings>> GetSystemSettings(int? id, QueryParam param, CancellationToken cancellation = default)
    {
        logger.LogInformation("Retrieving system settings with filter: {Filter}, page: {Page}, take: {Take}", param.Filter, param.Page, param.Take);

        await using var cxt = await dbContextFactory.CreateDbContextAsync(cancellation);

        var queries = cxt.AppSettings
                        .WhereIf(id.HasValue && id.Value > 0, x => x.Id == id)
                        .OrderByDescending(x => x.Code)
                            .ThenByDescending(x => x.Value)
                        .AsQueryable();

        if (!string.IsNullOrWhiteSpace(param.Filter))
        {
            string[] filterColums = ["Code", "Value"];

            queries = queries.StartWiths(param.Filter, filterColums);

        }

        var count = await queries.CountAsync(cancellation);

        if (param.Take > 0)
        {
            queries = queries.Skip(param.Page).Take(param.Take);
        }

        var items = await queries.AsNoTracking().ToListAsync(cancellation);

        return PagerExtensions.Create(items, count);

    }

    public async Task<ErrorOr<AppSettingDto>> GetAppSettingByIdAsync(int id, CancellationToken cancellation = default)
    {
        try
        {
            await using var cxt = await dbContextFactory.CreateDbContextAsync(cancellation);

            var item = await cxt.AppSettings
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellation);

            if (item is null)
                return Error.NotFound(description: "App setting not found.");

            return new AppSettingDto
            {
                Id = item.Id,
                Code = item.Code,
                Value = item.Value,
                Description = item.Description,
                IsActive = item.IsActive
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Error.Failure(description: ex.Message);
        }
    }

    public async Task<ErrorOr<AppSettingDto>> CreateAppSettingAsync(AppSettingDtoCreate request, CancellationToken cancellation = default)
    {
        try
        {
            await using var cxt = await dbContextFactory.CreateDbContextAsync(cancellation);

            var exists = await cxt.AppSettings.AnyAsync(x => x.Code == request.Code, cancellation);
            if (exists)
                return Error.Conflict(description: $"App setting with code '{request.Code}' already exists.");

            var entity = new AppSettings
            {
                Code = request.Code!,
                Value = request.Value!,
                Description = request.Description,
                IsActive = request.IsActive!.Value
            };

            cxt.AppSettings.Add(entity);
            await cxt.SaveChangesAsync(cancellation);

            return new AppSettingDto
            {
                Id = entity.Id,
                Code = entity.Code,
                Value = entity.Value,
                Description = entity.Description,
                IsActive = entity.IsActive
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Error.Failure(description: ex.Message);
        }
    }

    public async Task<ErrorOr<AppSettingDto>> UpdateAppSettingAsync(AppSettingDtoUpdate request, CancellationToken cancellation = default)
    {
        try
        {
            await using var cxt = await dbContextFactory.CreateDbContextAsync(cancellation);

            var entity = await cxt.AppSettings.FindAsync([request.Id], cancellation);
            if (entity is null)
                return Error.NotFound(description: "App setting not found.");

            var duplicate = await cxt.AppSettings
                .AnyAsync(x => x.Code == request.Code && x.Id != request.Id, cancellation);
            if (duplicate)
                return Error.Conflict(description: $"Another app setting with code '{request.Code}' already exists.");

            entity.Code = request.Code;
            entity.Value = request.Value;
            entity.Description = request.Description;
            entity.IsActive = request.IsActive;

            await cxt.SaveChangesAsync(cancellation);

            return new AppSettingDto
            {
                Id = entity.Id,
                Code = entity.Code,
                Value = entity.Value,
                Description = entity.Description,
                IsActive = entity.IsActive
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Error.Failure(description: ex.Message);
        }
    }

    public async Task<ErrorOr<bool>> DeleteAppSettingAsync(int id, CancellationToken cancellation = default)
    {
        try
        {
            await using var cxt = await dbContextFactory.CreateDbContextAsync(cancellation);

            var entity = await cxt.AppSettings.FindAsync(id, cancellation);

            if (entity is null)
                return Error.NotFound(description: "App setting not found.");

            cxt.AppSettings.Remove(entity);

            await cxt.SaveChangesAsync(cancellation);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Error.Failure(description: ex.Message);
        }
    }

    // ── UserLoanSize ───────────────────────────────────────────────────────────

    public async Task<Pager<VUserLoanSizes>> GetUserLoanSizesAsync(string? userRefId, QueryParam param, CancellationToken cancellation = default)
    {
        try
        {
            await using var cxt = await dbContextFactory.CreateDbContextAsync(cancellation);

            var queries = cxt.Set<VUserLoanSizes>().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(userRefId))
                queries = queries.Where(x => x.UserRefId == userRefId);

            if (param.IncludeInActive)
            {
                queries = queries.IgnoreQueryFilters();
            }
            queries = ApplyFilters(param, queries);

            queries = ApplyOrder(param, queries);

            var count = await queries.CountAsync(cancellation);

            if (param.Take > 0)
                queries = queries.Skip(param.Page).Take(param.Take);

            var items = await queries.AsNoTracking().ToListAsync(cancellation);

            return PagerExtensions.Create(items, count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            return PagerExtensions.Create<VUserLoanSizes>([], 0);
        }
    }

    private static IQueryable<VUserLoanSizes> ApplyFilters(QueryParam param, IQueryable<VUserLoanSizes> queries)
    {
        if (string.IsNullOrWhiteSpace(param.Filter)) return queries;

        var filterValue = param.Filter.Trim();

        if (string.IsNullOrWhiteSpace(param.FilterColumn))
        {

            var columnsToSearch = new[]
            {
                    nameof(VUserLoanSizes.Description),
                    nameof(VUserLoanSizes.MinUsdLoanSize),
                    nameof(VUserLoanSizes.MaxUsdLoanSize),
                    nameof(VUserLoanSizes.MinKhrLoanSize),
                    nameof(VUserLoanSizes.MaxKhrLoanSize)
                };

            queries = queries.StartWiths(filterValue, columnsToSearch);


            return queries;
        }

        var property = typeof(VUserLoanSizes).MapField(param.FilterColumn);

        if (property != null)
        {
            queries = queries.StartWith(filterValue, property);
        }

        return queries;
    }

    private static IQueryable<VUserLoanSizes> ApplyOrder(QueryParam param, IQueryable<VUserLoanSizes> queries)
    {
        var isDescending = param.SortDescending;

        if (!string.IsNullOrWhiteSpace(param.SortColumn))
        {
            queries = isDescending ? queries.OrderByDescending(x => EF.Property<object>(x, param.SortColumn))
                                   : queries.OrderBy(x => EF.Property<object>(x, param.SortColumn!));
        }
        else
        {
            queries = isDescending ? queries.OrderByDescending(x => x.MaxKhrLoanSize)
                                                .ThenByDescending(x => x.MaxUsdLoanSize)
                                   : queries.OrderBy(x => x.MinKhrLoanSize)
                                            .ThenBy(x => x.MinUsdLoanSize);

        }

        return queries;
    }

    public async Task<ErrorOr<UserLoanSizeDto>> GetUserLoanSizeByIdAsync(long id, CancellationToken cancellation = default)
    {
        try
        {
            await using var cxt = await dbContextFactory.CreateDbContextAsync(cancellation);

            var item = await cxt.UserLoanSizes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellation);

            if (item is null)
                return Error.NotFound(description: "User loan size not found.");

            return new UserLoanSizeDto
            {
                Id = item.Id,
                UserRefId = item.UserRefId,
                MinUsdLoanSize = item.MinUsdLoanSize,
                MaxUsdLoanSize = item.MaxUsdLoanSize,
                MinKhrLoanSize = item.MinKhrLoanSize,
                MaxKhrLoanSize = item.MaxKhrLoanSize,
                StartDate = item.StartDate,
                EndDate = item.EndDate,
                Description = item.Description,
                IsActive = item.IsActive,
                EditSeq = item.EditSeq
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Error.Failure(description: ex.Message);
        }
    }

    public async Task<ErrorOr<VUserLoanSizes>> GetUserLoanSizeByUserAsync(string? userId, CancellationToken cancellation = default)
    {
        try
        {
            await using var cxt = await dbContextFactory.CreateDbContextAsync(cancellation);

            var item = await cxt.Set<VUserLoanSizes>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserRefId == userId, cancellation);

            if (item is null)
                return Error.NotFound(description: "User loan size not found.");

            return item;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Error.Failure(description: ex.Message);
        }
    }


    public async Task<ErrorOr<UserLoanSizeDto>> CreateUserLoanSizeAsync(
        UserLoanSizeDtoCreate request, CancellationToken cancellation = default)
    {
        try
        {
            await using var cxt = await dbContextFactory.CreateDbContextAsync(cancellation);

            var exists = await cxt.UserLoanSizes
                        .IgnoreQueryFilters()
                        .AnyAsync(x => x.UserRefId == request.UserRefId, cancellation);
            if (exists)
                return Error.Conflict(description: "A loan size record already exists for this user.");

            var entity = new UserLoanSize
            {
                UserRefId = request.UserRefId,
                MinUsdLoanSize = request.MinUsdLoanSize,
                MaxUsdLoanSize = request.MaxUsdLoanSize,
                MinKhrLoanSize = request.MinKhrLoanSize,
                MaxKhrLoanSize = request.MaxKhrLoanSize,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Description = request.Description,
                IsActive = request.IsActive
            };

            cxt.UserLoanSizes.Add(entity);

            await cxt.SaveChangesAsync(cancellation);

            return new UserLoanSizeDto
            {
                Id = entity.Id,
                UserRefId = entity.UserRefId,
                MinUsdLoanSize = entity.MinUsdLoanSize,
                MaxUsdLoanSize = entity.MaxUsdLoanSize,
                MinKhrLoanSize = entity.MinKhrLoanSize,
                MaxKhrLoanSize = entity.MaxKhrLoanSize,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Description = entity.Description,
                IsActive = entity.IsActive,
                EditSeq = entity.EditSeq
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Error.Failure(description: ex.Message);
        }
    }

    public async Task<ErrorOr<UserLoanSizeDto>> UpdateUserLoanSizeAsync(
        UserLoanSizeDtoUpdate request, CancellationToken cancellation = default)
    {
        try
        {
            await using var cxt = await dbContextFactory.CreateDbContextAsync(cancellation);

            var entity = await cxt.UserLoanSizes
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.EditSeq == request.EditSeq, cancellation);

            if (entity is null)
                return Error.NotFound(description: "User loan size not found or has been modified by another user.");

            entity.MinUsdLoanSize = request.MinUsdLoanSize;
            entity.MaxUsdLoanSize = request.MaxUsdLoanSize;
            entity.MinKhrLoanSize = request.MinKhrLoanSize;
            entity.MaxKhrLoanSize = request.MaxKhrLoanSize;
            entity.StartDate = request.StartDate;
            entity.EndDate = request.EndDate;
            entity.Description = request.Description;
            entity.IsActive = request.IsActive;
            entity.EditSeq = request.EditSeq + 1;

            await cxt.SaveChangesAsync(cancellation);

            return new UserLoanSizeDto
            {
                Id = entity.Id,
                UserRefId = entity.UserRefId,
                MinUsdLoanSize = entity.MinUsdLoanSize,
                MaxUsdLoanSize = entity.MaxUsdLoanSize,
                MinKhrLoanSize = entity.MinKhrLoanSize,
                MaxKhrLoanSize = entity.MaxKhrLoanSize,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Description = entity.Description,
                IsActive = entity.IsActive,
                EditSeq = entity.EditSeq
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Error.Failure(description: ex.Message);
        }
    }

    public async Task<ErrorOr<bool>> DeleteUserLoanSizeAsync(long? id, CancellationToken cancellation = default)
    {
        try
        {
            await using var cxt = await dbContextFactory.CreateDbContextAsync(cancellation);

            var entity = await cxt.UserLoanSizes.FindAsync(id, cancellation);

            if (entity is null)
                return Error.NotFound(description: "User loan size not found.");

            cxt.UserLoanSizes.Remove(entity);

            await cxt.SaveChangesAsync(cancellation);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Error.Failure(description: ex.Message);
        }
    }

    public async Task<ErrorOr<bool>> ToggleUserLoanSizeActiveAsync(long id, string userRefId, CancellationToken cancellation = default)
    {
        try
        {
            await using var cxt = await dbContextFactory.CreateDbContextAsync(cancellation);

            var entity = await cxt.UserLoanSizes
                .AsNoTracking()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == id && x.UserRefId == userRefId, cancellation);

            if (entity is null)
                return Error.NotFound(description: "User loan size not found.");

            var updated = await cxt.UserLoanSizes
                .IgnoreQueryFilters()
                .Where(x => x.Id == id && x.UserRefId == userRefId && x.EditSeq == entity.EditSeq)
                .ExecuteUpdateAsync(props => props
                    .SetProperty(x => x.IsActive, !entity.IsActive)
                    .SetProperty(x => x.EditSeq, entity.EditSeq + 1), cancellation);

            return updated > 0
                ? true
                : Error.Conflict(description: "Update failed — record may have been modified concurrently.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Error.Failure(description: ex.Message);
        }
    }

    // ── CollateralSubType ──────────────────────────────────────────────────────

    public async Task<Pager<CollateralSubTypeDto>> GetCollateralSubTypesAsync(QueryParam param, CancellationToken cancellation = default)
    {
        try
        {
            await using var cxt = await dbContextFactory.CreateDbContextAsync(cancellation);

            var query = cxt.CollateralSubTypes.AsNoTracking();

            if (!param.IncludeInActive)
                query = query.Where(x => x.IsActive == true);

            if (!string.IsNullOrWhiteSpace(param.Filter))
                query = query.Where(x =>
                    EF.Functions.Like(x.CollateralSubTypeName!, $"%{param.Filter}%") ||
                    EF.Functions.Like(x.CollateralSubTypeNameInKhmer!, $"%{param.Filter}%"));

            if (param.SortDescending)
                query = query.OrderByDescending(x => EF.Property<object>(x, param.SortColumn!));

            var count = await query.CountAsync(cancellation);

            var items = await query
                .Skip(param.Page)
                .Take(param.Take)
                .Select(x => new CollateralSubTypeDto
                {
                    Id = x.Id,
                    CollateralSubTypeName = x.CollateralSubTypeName,
                    CollateralSubTypeNameInKhmer = x.CollateralSubTypeNameInKhmer,
                    CollateralTypeRefId = x.CollateralTypeRefId,
                    ValueRate = x.ValueRate,
                    LoanSizeAmuntMin = x.LoanSizeAmuntMin,
                    LoanSizeAmuntMax = x.LoanSizeAmuntMax,
                    IsActive = x.IsActive
                }).ToListAsync(cancellation);

            return new Pager<CollateralSubTypeDto>(items, count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            return new Pager<CollateralSubTypeDto>([], 0);
        }
    }

}
