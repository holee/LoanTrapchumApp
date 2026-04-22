using Applications.Interfaces.setting;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Settings;
using Dapper;
using Domains.Models.Collaterals;
using Domains.Models.Loans;
using Domains.Models.Settings;
using Infrastructures.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Implements;

/// <summary>
/// Legacy implementation of IAppSettingService used by SettingService (ISettingServiceImpl).
/// New code should use CQRS handlers via IAppSettingRepository instead.
/// </summary>
public sealed class LegacyAppSettingService(
    IDbContextFactory<DataContext> dbContextFactory,
    IDapperConnectionFactory dapper,
    ILogger<LegacyAppSettingService> logger) : IAppSettingService
{
    public async Task<Result<string>> Delete(long? id, CancellationToken cancellation = default)
    {
        try
        {
            await using var cxt = await dbContextFactory.CreateDbContextAsync(cancellation);
            var entity = await cxt.AppSettings.FindAsync([(int?)id], cancellation);
            if (entity is null)
                return Result.Failure<string>(ErrorResponse.NotFound("App setting not found."));
            cxt.AppSettings.Remove(entity);
            await cxt.SaveChangesAsync(cancellation);
            return Result.Success("Deleted.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Result.Failure<string>(ErrorResponse.Failure(ex.Message));
        }
    }

    public async Task<Result<string>> Save(AppSettings entity, CancellationToken cancellation = default)
    {
        try
        {
            await using var cxt = await dbContextFactory.CreateDbContextAsync(cancellation);
            cxt.AppSettings.Add(entity);
            await cxt.SaveChangesAsync(cancellation);
            return Result.Success("Saved.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Result.Failure<string>(ErrorResponse.Failure(ex.Message));
        }
    }

    public async Task<Result<string>> Update(AppSettings entity)
    {
        try
        {
            await using var cxt = await dbContextFactory.CreateDbContextAsync();
            cxt.AppSettings.Update(entity);
            await cxt.SaveChangesAsync();
            return Result.Success("Updated.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Result.Failure<string>(ErrorResponse.Failure(ex.Message));
        }
    }

    public async Task<Result<AppSettings>> GetSysSettingByCode(string code)
    {
        try
        {
            await using var cxt = await dbContextFactory.CreateDbContextAsync();
            var item = await cxt.AppSettings.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Code == code);
            if (item is null)
                return Result.Failure<AppSettings>(ErrorResponse.NotFound($"Setting '{code}' not found."));
            return Result.Success(item);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Result.Failure<AppSettings>(ErrorResponse.Failure(ex.Message));
        }
    }

    public async Task<Result<AppSettings>> GetSysSettingByCode(int? id)
    {
        try
        {
            await using var cxt = await dbContextFactory.CreateDbContextAsync();
            var item = await cxt.AppSettings.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (item is null)
                return Result.Failure<AppSettings>(ErrorResponse.NotFound("Setting not found."));
            return Result.Success(item);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Result.Failure<AppSettings>(ErrorResponse.Failure(ex.Message));
        }
    }

    public async Task<Result<IEnumerable<AppSettings>>> GetSysSettings(string? filter = null)
    {
        try
        {
            await using var cxt = await dbContextFactory.CreateDbContextAsync();
            var query = cxt.AppSettings.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(filter))
                query = query.Where(x => x.Code.Contains(filter) || x.Value.Contains(filter));
            var items = await query.ToListAsync();
            return Result.Success<IEnumerable<AppSettings>>(items);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Result.Failure<IEnumerable<AppSettings>>(ErrorResponse.Failure(ex.Message));
        }
    }

    public async Task<Result<IEnumerable<T>>> GetSysSettings<T>(string? filterValue = null) where T : class
    {
        try
        {
            var rsp = await GetSysSettings(filterValue);
            if (!rsp.IsSuccess)
                return Result.Failure<IEnumerable<T>>(rsp.Errors);
            var mapped = rsp.Data!.OfType<T>();
            return Result.Success(mapped);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Result.Failure<IEnumerable<T>>(ErrorResponse.Failure(ex.Message));
        }
    }

    public async Task<Result<string>> DeActivateUserLoanSizeAsync(long? id, string? userRefId, CancellationToken cancellation = default)
    {
        try
        {
            await using var cxt = await dbContextFactory.CreateDbContextAsync(cancellation);
            var entity = await cxt.UserLoanSizes.FindAsync([id], cancellation);
            if (entity is null)
                return Result.Failure<string>(ErrorResponse.NotFound("User loan size not found."));
            entity.IsActive = false;
            await cxt.SaveChangesAsync(cancellation);
            return Result.Success("Deactivated.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Result.Failure<string>(ErrorResponse.Failure(ex.Message));
        }
    }

    public async Task<Result<string>> CreateUserLoanSizeAsync(UserLoanSize model, CancellationToken cancellation = default)
    {
        try
        {
            await using var cxt = await dbContextFactory.CreateDbContextAsync(cancellation);
            cxt.UserLoanSizes.Add(model);
            await cxt.SaveChangesAsync(cancellation);
            return Result.Success("Created.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Result.Failure<string>(ErrorResponse.Failure(ex.Message));
        }
    }

    public async Task<Result<string>> EditUserLoanSizeAsync(UserLoanSize model, CancellationToken cancellation = default)
    {
        try
        {
            await using var cxt = await dbContextFactory.CreateDbContextAsync(cancellation);
            cxt.UserLoanSizes.Update(model);
            await cxt.SaveChangesAsync(cancellation);
            return Result.Success("Updated.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Result.Failure<string>(ErrorResponse.Failure(ex.Message));
        }
    }

    public async Task<Pager<VUserLoanSizes>> GetUserLoanSizes(string? userRefId, QueryParam q, CancellationToken cancellation = default)
    {
        try
        {
            using var db = dapper.CreateConnection();
            var filter = string.IsNullOrWhiteSpace(q.Filter) ? null : q.Filter;
            var sql = @"
                SELECT Id, UserRefId, FullName, BranchId, BranchName,
                       MinUsdLoanSize, MaxUsdLoanSize, MinKhrLoanSize, MaxKhrLoanSize,
                       StartDate, EndDate, Description, IsActive, EditSeq
                FROM VUserLoanSizes
                WHERE (@userRefId IS NULL OR UserRefId = @userRefId)
                  AND (@filter IS NULL OR FullName LIKE N'%' + @filter + '%')
                ORDER BY Id
                OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY;

                SELECT COUNT(*) FROM VUserLoanSizes
                WHERE (@userRefId IS NULL OR UserRefId = @userRefId)
                  AND (@filter IS NULL OR FullName LIKE N'%' + @filter + '%');";

            var p = new DynamicParameters();
            p.Add("@userRefId", userRefId);
            p.Add("@filter", filter);
            p.Add("@skip", q.Page);
            p.Add("@take", q.PageSize > 0 ? q.PageSize : 15);

            using var multi = await db.QueryMultipleAsync(sql, p);
            var items = (await multi.ReadAsync<VUserLoanSizes>()).ToList();
            var count = await multi.ReadFirstAsync<int>();
            return new Pager<VUserLoanSizes>(items, count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new Pager<VUserLoanSizes>([], 0);
        }
    }

    public async Task<Result<VUserLoanSizes>> GetLoanSizeByUserId(string? userRefId, CancellationToken cancellation = default)
    {
        try
        {
            using var db = dapper.CreateConnection();
            var sql = "SELECT TOP 1 * FROM VUserLoanSizes WHERE UserRefId = @userRefId AND IsActive = CAST(1 AS BIT)";
            var item = await db.QueryFirstOrDefaultAsync<VUserLoanSizes>(sql, new { userRefId });
            if (item is null)
                return Result.Failure<VUserLoanSizes>(ErrorResponse.NotFound("Loan size not found."));
            return Result.Success(item);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Result.Failure<VUserLoanSizes>(ErrorResponse.Failure(ex.Message));
        }
    }

    public async Task<Result<UserLoanSize>> GetLoanSizeById(long? id, string? userId, CancellationToken cancellation = default)
    {
        try
        {
            await using var cxt = await dbContextFactory.CreateDbContextAsync(cancellation);
            var item = await cxt.UserLoanSizes.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellation);
            if (item is null)
                return Result.Failure<UserLoanSize>(ErrorResponse.NotFound("Loan size not found."));
            return Result.Success(item);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Result.Failure<UserLoanSize>(ErrorResponse.Failure(ex.Message));
        }
    }

    public async Task<Pager<CollateralSubType>> GetCollateralSubTypes(QueryParam param, CancellationToken cancellation = default)
    {
        try
        {
            await using var cxt = await dbContextFactory.CreateDbContextAsync(cancellation);
            var query = cxt.CollateralSubTypes.AsNoTracking();
            var count = await query.CountAsync(cancellation);
            if (param.PageSize > 0)
                query = query.Skip(param.Page).Take(param.PageSize);
            var items = await query.ToListAsync(cancellation);
            return new Pager<CollateralSubType>(items, count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new Pager<CollateralSubType>([], 0);
        }
    }
}
