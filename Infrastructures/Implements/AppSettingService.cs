using Applications.Extensions;
using Applications.Interfaces.setting;
using Applications.Responses;
using Applications.Responses.Message;
using Domains.Models.Loans;
using Domains.Models.Settings;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Implements;

public sealed class AppSettingService(IDbContextFactory<DataContext> dbContextFactory) : IAppSettingService
{
    public Task<Result<string>> Delete(Guid? id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<string>> Save(AppSettings entity)
    {
        throw new NotImplementedException();
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

        await using var cxt= await dbContextFactory.CreateDbContextAsync();

        var queries = cxt.AppSettings
                            .WhereIf(!string.IsNullOrEmpty(filter), x => x.Description!.Contains(filter) ||
                                         x.Code.Contains(filter) ||
                                         x.Value.Contains(filter));
                             
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


    public Task<Result<string>> ApplyUserLoanSizeAsync(UserLoanSize model)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<string>> ApplyUserLoanSizeAsync(UserLoanSize model,CancellationToken cancellation=default)
    {
        try
        {
            await using var cxt = await dbContextFactory.CreateDbContextAsync(cancellation);

            cxt.UserLoanSizes.Add(model);

            await cxt.SaveChangesAsync(cancellation);

            return Result.Success<string>(Messages.DataCreated);
        }
        catch (Exception ex)
        {

            return Result.Failure<string>(ErrorResponse.ServerError(ex.Message));
        }
       
    }

    public async Task<Result<string>> DeActivateUserLoanSizeAsync(long? id,string? userRefId,CancellationToken cancellation=default)
    {
        await using var cxt = await dbContextFactory.CreateDbContextAsync(cancellation);

        var existing=await cxt.UserLoanSizes
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == id && 
                                             x.UserRefId==userRefId);  

        if(existing is null)
        {
            return Result.Failure<string>(ErrorResponse.NotFound(Messages.DataNotFound));
        }

        var newValue = !existing.IsActive;
        var editSeq=existing.EditSeq +1 ;
        var updated = await cxt.UserLoanSizes
                        .Where(x => x.Id == id &&
                                    x.UserRefId == userRefId && 
                                    x.EditSeq==existing.EditSeq)
                        .ExecuteUpdateAsync(props =>props.SetProperty(x=>x.IsActive, newValue)
                                                       .SetProperty(x=>x.EditSeq,editSeq));

        return updated > 0 ? 
               Result.Success<string>(new("Data was changed")) :
               Result.Failure<string>(new(Messages.DataNotFound));
    }
}