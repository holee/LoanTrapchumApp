using Applications.Requests.Settings;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Settings;
using Domains.Models.Loans;
using Domains.Models.Settings;

namespace Applications.Interfaces.setting;

public interface IAppSettingService
{
    Task<Result<string>> Delete(long? id, CancellationToken cancellation = default);
    Task<Result<string>> Save(AppSettings entity, CancellationToken cancellation = default);
    Task<Result<string>> Update(AppSettings entity);
    Task<Result<AppSettings>> GetSysSettingByCode(string code);
    Task<Result<AppSettings>> GetSysSettingByCode(int? id);
    Task<Result<IEnumerable<AppSettings>>> GetSysSettings(string? filter=null);
    Task<Result<string>> DeActivateUserLoanSizeAsync(long? id,string? userRefId,CancellationToken cancellation = default);
    Task<Result<IEnumerable<T>>> GetSysSettings<T>(string? filterValue = null) where T : class;
    Task<Result<string>> CreateUserLoanSizeAsync(UserLoanSize model, CancellationToken cancellation = default);
    Task<Pager<VUserLoanSizes>> GetUserLoanSizes(string? userRefId, QueryParam q, CancellationToken cancellation = default);
    Task<Result<VUserLoanSizes>> GetLoanSizeByUserId(string? userRefId, CancellationToken cancellation = default);
    Task<Result<UserLoanSize>> GetLoanSizeById(long? id,string? userId, CancellationToken cancellation = default);
    Task<Result<string>> EditUserLoanSizeAsync(UserLoanSize model, CancellationToken cancellation = default);
}
