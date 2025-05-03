using Applications.Interfaces.setting;
using Applications.Responses;
using Domains.Models.Loans;
using Domains.Models.Settings;

namespace Infrastructures.Implements;

public sealed class AppSettingService : IAppSettingService 
{
    public Task<Result<string>> Delete(Guid? id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<string>> Save(SysSettings entity)
    {
        throw new NotImplementedException();
    }

    public Task<Result<string>> Update(SysSettings entity)
    {
        throw new NotImplementedException();
    }

    public Task<Result<SysSettings>> GetSysSettingByCode(string code)
    {
        throw new NotImplementedException();
    }

    public Task<Result<SysSettings>> GetSysSettingByCode(int? id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<SysSettings>>> GetSysSettings(string? filterValue = null)
    {
        throw new NotImplementedException();
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