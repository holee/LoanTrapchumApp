using Applications.Extensions;
using Applications.Interfaces.setting;
using Applications.Responses;
using Applications.Responses.Auths;
using Applications.Responses.Message;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Settings;
using Domains.Models.Loans;
using Domains.Models.Settings;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Implements;

public sealed class AppSettingService(IDbContextFactory<DataContext> dbContextFactory, ILogger<AppSettingService> logger) : IAppSettingService
{
    public async Task<Result<string>> Delete(long? id, CancellationToken cancellation = default)
    {
        await using var cxt = await dbContextFactory.CreateDbContextAsync();

        var existing = await cxt.AppSettings.FindAsync(id);

        if (existing is null)
        {
            return Result.Failure<string>(ErrorResponse.NotFound(Messages.DataNotFound));
        }

        cxt.AppSettings.Remove(existing);

        await cxt.SaveChangesAsync(cancellation);

        return Result.Success("Setting was removed.");
    }

    public async Task<Result<string>> Save(AppSettings entity, CancellationToken cancellation = default)
    {
        await using var cxt = await dbContextFactory.CreateDbContextAsync();

        cxt.AppSettings.Add(entity);

        await cxt.SaveChangesAsync(cancellation);

        return Result.Success(Messages.DataCreated);
    }

    public Task<Result<string>> Update(AppSettings entity)
    {
        throw new NotImplementedException();
    }

    public Task<Result<AppSettings>> GetSysSettingByCode(string code)
    {
        throw new NotImplementedException();
    }

    public Task<Result<AppSettings>> GetSysSettingByCode(int? id)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<IEnumerable<AppSettings>>> GetSysSettings(string? filter = null)
    {

        await using var cxt = await dbContextFactory.CreateDbContextAsync();

        var queries = cxt.AppSettings
                            .WhereIf(filter.IsNotNullOrEmpty(),
                                         x => x.Description!.Contains(filter!) ||
                                         x.Code.Contains(filter!) ||
                                         x.Value.Contains(filter!));

        var items = await queries.AsNoTracking().ToListAsync();

        return Result.Success<IEnumerable<AppSettings>>(items);
    }

    public async Task<Result<IEnumerable<T>>> GetSysSettings<T>(string? filterValue = null)
        where T : class
    {
        await using var cxt = await dbContextFactory.CreateDbContextAsync();

        IQueryable<AppSettings> queries = cxt.AppSettings;

        if (!string.IsNullOrEmpty(filterValue))
        {
            queries = queries.Where(x => x.Description!.Contains(filterValue) ||
                                         x.Code.Contains(filterValue) ||
                                         x.Value.Contains(filterValue));
        }

        var items = await queries
                                    .ProjectToType<T>()
                                    .AsNoTracking()
                                    .ToListAsync();

        return Result.Success<IEnumerable<T>>(items);
    }

    public async Task<Result<string>> CreateUserLoanSizeAsync(UserLoanSize model, CancellationToken cancellation = default)
    {
        try
        {
            logger.LogInformation("inserting new user loan size......");

            await using var cxt = await dbContextFactory.CreateDbContextAsync(cancellation);

            var existings = await cxt.UserLoanSizes.AnyAsync(x => x.UserRefId == model.UserRefId);

            if (existings) 
            {
                Result.Failure(ErrorResponse.Conflict(Messages.DataExist));            
            }

            cxt.UserLoanSizes.Add(model);

            await cxt.SaveChangesAsync(cancellation);

            return Result.Success<string>(Messages.DataCreated);
        }
        catch (Exception ex)
        {

            logger.LogError(ex.Message, ex);

            return Result.Failure<string>(ErrorResponse.ServerError(ex.Message));
        }

    }

    public async Task<Result<string>> EditUserLoanSizeAsync(UserLoanSize model, CancellationToken cancellation = default)
    {
        try
        {
            logger.LogInformation("inserting new user loan size......");

            await using var cxt = await dbContextFactory.CreateDbContextAsync(cancellation);

            var existings = await cxt.UserLoanSizes.AnyAsync(x => x.Id==model.Id &&  
                                                                  x.UserRefId == model.UserRefId &&
                                                                  x.EditSeq==model.EditSeq);

            if (!existings)
            {
               return Result.Failure<string>(ErrorResponse.Conflict(Messages.DataNotFound)); 
            }

            cxt.UserLoanSizes.Update(model);

            await cxt.SaveChangesAsync(cancellation);

            return Result.Success<string>(Messages.DataCreated);
        }
        catch (Exception ex)
        {

            logger.LogError(ex.Message, ex);

            return Result.Failure<string>(ErrorResponse.ServerError(ex.Message));
        }

    }


    public async Task<Result<VUserLoanSizes>> GetLoanSizeByUserId(string? userRefId, CancellationToken cancellation = default)
    {
        try
        {
            await using var cxt = await dbContextFactory.CreateDbContextAsync(cancellation);

            var item = await cxt.Database
                                  .SqlQuery<VUserLoanSizes>($"SELECT * FROM VUserLoanSizes WHERE UserRefId={userRefId}")
                                  .AsNoTracking()
                                  .FirstOrDefaultAsync(cancellation);

            if (item is null)
            {
                return Result.Failure<VUserLoanSizes>(ErrorResponse.NotFound(Messages.DataNotFound));
            }

            return Result.Success(item);

        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, ex);

            return Result.Failure<VUserLoanSizes>(ErrorResponse.ServerError(ex.Message));
        }

    }

    public async Task<Result<UserLoanSize>> GetLoanSizeById(long? id,string? userId, CancellationToken cancellation = default)
    {
        try 
        {
            await using var cxt = await dbContextFactory.CreateDbContextAsync(cancellation);

            var item = await cxt.UserLoanSizes
                                .FirstOrDefaultAsync(x=>x.Id==id && x.UserRefId==userId);

            if (item is null)
            {
                return Result.Failure<UserLoanSize>(ErrorResponse.NotFound(Messages.DataNotFound));
            }

            return Result.Success(item);

        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, ex);

            return Result.Failure<UserLoanSize>(ErrorResponse.ServerError(ex.Message));
        }

    }




    public async Task<Pager<VUserLoanSizes>> GetUserLoanSizes(string? userRefId, QueryParam q, CancellationToken cancellation = default)
    {
        try
        {
            logger.LogInformation("getting user loan size.....");

            await using var cxt = await dbContextFactory.CreateDbContextAsync(cancellation);

            var queries = cxt.Database
                                  .SqlQuery<VUserLoanSizes>($"SELECT * FROM VUserLoanSizes")
                                  .WhereIf(userRefId.IsNotNullOrEmpty(), u => u.UserRefId == userRefId)
                                  .WhereIf(!q.IsIncludeInActive, x => x.IsActive == true);

            if (q.Filter.IsNotNullOrEmpty())
            {
                Console.WriteLine(q.Filter);

                logger.LogInformation($"filter with {q.Filter}");

                queries = queries.Where(x => EF.Functions.Like(x.FullName, $"%{q.Filter}%") ||
                                             EF.Functions.Like(x.MinKhrLoanSize.ToString(), $"%{q.Filter}%") ||
                                             EF.Functions.Like(x.MaxKhrLoanSize.ToString(), $"%{q.Filter}%") ||
                                             EF.Functions.Like(x.MinUsdLoanSize.ToString(), $"%{q.Filter}%") ||
                                             EF.Functions.Like(x.MaxUsdLoanSize.ToString(), $"%{q.Filter}%") ||
                                             EF.Functions.Like(x.BranchName, $"%{q.Filter}%"));
            }

            if (q.SortDescending)
            {
                queries = queries.OrderByDescending(x => EF.Property<object>(x, q.SortColumn));
            }
            var counts = await queries.CountAsync(cancellation);

            if (q.PageSize > 0)
            {
                queries = queries.Skip(q.Skip).Take(q.PageSize);
            }

            var items = await queries.AsNoTracking().ToListAsync(cancellation);


            return new Pager<VUserLoanSizes>(items, counts);

        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, ex);

            return new Pager<VUserLoanSizes>([], 0);
        }

    }



    public async Task<Result<string>> DeActivateUserLoanSizeAsync(long? id, string? userRefId, CancellationToken cancellation = default)
    {
        await using var cxt = await dbContextFactory.CreateDbContextAsync(cancellation);

        var existing = await cxt.UserLoanSizes
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == id &&
                                             x.UserRefId == userRefId);

        if (existing is null)
        {
            return Result.Failure<string>(ErrorResponse.NotFound(Messages.DataNotFound));
        }

        var newValue = !existing.IsActive;
        var editSeq = existing.EditSeq + 1;
        var updated = await cxt.UserLoanSizes
                        .Where(x => x.Id == id &&
                                    x.UserRefId == userRefId &&
                                    x.EditSeq == existing.EditSeq)
                        .ExecuteUpdateAsync(props => props.SetProperty(x => x.IsActive, newValue)
                                                       .SetProperty(x => x.EditSeq, editSeq));

        return updated > 0 ?
               Result.Success<string>(new("Data was changed")) :
               Result.Failure<string>(new(Messages.DataNotFound));
    }
}