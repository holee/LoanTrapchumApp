using Applications.Extensions.Identity;
using Applications.Responses;
using Domains.Models.Identity;
using Domains.Models.Loans;
using Domains.Models.Settings;

namespace Applications.Interfaces.setting;

public interface ISettingService
{
    Task<Result<string>> Delete(int id);
    Task<Result<string>> Save(SysSettings entity);
    Task<Result<string>> Update(SysSettings entity);
    Task<Result<SysSettings>> GetSysSettingByCode(string code);
    Task<Result<SysSettings>> GetSysSettingByCode(int id);
    Task<Result<IEnumerable<SysSettings>>> GetSysSettings(string? filterValue=null);
    Task<Result<IEnumerable<AprovePerson>>> GetAproveUserAsync(int branchId,string? filterValue = null);
    Task<Result<string>> ApplyUserLoanSizeAsync(UserLoanSize model);
    Task<Result<string>> DeActivateUserLoanSizeAsync(long? id);
}
