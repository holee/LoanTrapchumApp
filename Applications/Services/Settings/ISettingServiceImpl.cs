using Applications.Requests.Settings;
using Applications.Responses;
using Domains.Models.Loans;
using Domains.Models.Settings;

namespace Applications.Services.Settings
{
    public interface ISettingServiceImpl
    {
        Task<Result<string>> ApplyUserLoanSizeAsync(UserLoanSizeDtoCreate model);
        Task<Result<string>> DeActivateUserLoanSizeAsync(long? id, string userRefId, CancellationToken cancellation = default);
        Task<Result<string>> Delete(Guid? id);
        Task<Result<AppSettings>> GetSysSettingByCode(int? id);
        Task<Result<AppSettings>> GetSysSettingByCode(string code);
        Task<Result<IEnumerable<AppSettings>>> GetSysSettings(string? filterValue = null);
        Task<Result<string>> Save(AppSettings entity);
        Task<Result<string>> Update(AppSettings entity);

        Task<Result<IEnumerable<T>>> GetSysSettings<T>(string? filterValue = null)
            where T:class;
    }
}
