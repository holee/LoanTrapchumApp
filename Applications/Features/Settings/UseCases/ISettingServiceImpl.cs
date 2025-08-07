using Applications.Requests.Settings;
using Applications.Responses;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Settings;
using Domains.Models.Collaterals;
using Domains.Models.Loans;
using Domains.Models.Settings;

namespace Applications.Features.Settings.UseCases
{
    public interface ISettingServiceImpl
    {
        Task<Result<string>> CreateUserLoanSizeAsync(UserLoanSizeDtoCreate model);
        Task<Result<string>> DeActivateUserLoanSizeAsync(long? id, string userRefId, CancellationToken cancellation = default);
        Task<Result<string>> Delete(long? id);
        Task<Result<AppSettings>> GetSysSettingByCode(int? id);
        Task<Result<AppSettings>> GetSysSettingByCode(string code);
        Task<Result<IEnumerable<AppSettings>>> GetSysSettings(string? filterValue = null);
        Task<Result<string>> Save(AppSettings entity);
        Task<Result<string>> Update(AppSettings entity);

        Task<Result<IEnumerable<T>>> GetSysSettings<T>(string? filterValue = null)
            where T:class;
        Task<Result<VUserLoanSizes>> GetLoanSizeByUserId(string? userRefId, CancellationToken cancellation = default);
        Task<Pager<VUserLoanSizes>> GetUserLoanSizes(string? userRefId, QueryParam q, CancellationToken cancellation = default);
        Task<Result<UserLoanSizeDtoCreate>> GetEditLoanSizeByUserId(long? id,string? userId, CancellationToken cancellation = default);
        Task<Result<string>> EditUserLoanSizeAsync(UserLoanSizeDtoCreate req, CancellationToken cancellation = default);
        Task<Pager<CollateralSubType>> GetColleralSubTypes(QueryParam param, CancellationToken cancellation = default);
    }
}
