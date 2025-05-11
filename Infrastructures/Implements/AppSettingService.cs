using Applications.Interfaces.setting;
using Applications.Responses;
using Domains.Models.Loans;
using Domains.Models.Settings;
using Infrastructures.DataAccess;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Implements;

public sealed class AppSettingService(ApplicationDbContext context) : IAppSettingService
{
    private readonly ApplicationDbContext _context = context;

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

    public async Task<Result<IEnumerable<AppSettings>>> GetSysSettings(string? filterValue = null)
    {
        IQueryable<AppSettings> queries = _context.AppSettings;

        if (!string.IsNullOrEmpty(filterValue))
        {
            queries = queries.Where(x => x.Description!.Contains(filterValue) ||
                                         x.Code.Contains(filterValue) ||
                                         x.Value.Contains(filterValue));
        }

        var items = await queries.AsNoTracking().ToListAsync();

        return Result.Success<IEnumerable<AppSettings>>(items);
    }

    public async Task<Result<IEnumerable<T>>> GetSysSettings<T>(string? filterValue = null)
        where T : class
    {
        IQueryable<AppSettings> queries = _context.AppSettings;

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

    public Task<Result<string>> DeActivateUserLoanSizeAsync(Guid? id)
    {
        throw new NotImplementedException();
    }
}