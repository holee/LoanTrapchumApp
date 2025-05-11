using Applications.Extensions.Identity;
using Applications.Responses;
using Domains.Models.Identity;
using Domains.Models.Loans;
using Domains.Models.Settings;

namespace Applications.Interfaces.setting;

public interface IAppSettingService
{
    Task<Result<string>> Delete(Guid? id);
    Task<Result<string>> Save(AppSettings entity);
    Task<Result<string>> Update(AppSettings entity);
    Task<Result<AppSettings>> GetSysSettingByCode(string code);
    Task<Result<AppSettings>> GetSysSettingByCode(int? id);
    Task<Result<IEnumerable<AppSettings>>> GetSysSettings(string? filterValue=null);
    Task<Result<string>> ApplyUserLoanSizeAsync(UserLoanSize model);
    Task<Result<string>> DeActivateUserLoanSizeAsync(Guid? id);
    Task<Result<IEnumerable<T>>> GetSysSettings<T>(string? filterValue = null)
        where T : class;
}
