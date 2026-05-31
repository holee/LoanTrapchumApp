using ErrorOr;

namespace Applications.Interfaces.Settings;

public interface IAppSettingRepository
{
    // AppSettings
    Task<ErrorOr<IEnumerable<AppSettingDto>>> GetAppSettingsAsync(string? filter = null, CancellationToken cancellation = default);
    Task<ErrorOr<AppSettingDto>> GetAppSettingByIdAsync(int id, CancellationToken cancellation = default);
    Task<ErrorOr<AppSettingDto>> CreateAppSettingAsync(AppSettingDtoCreate request, CancellationToken cancellation = default);
    Task<ErrorOr<AppSettingDto>> UpdateAppSettingAsync(AppSettingDtoUpdate request, CancellationToken cancellation = default);
    Task<ErrorOr<bool>> DeleteAppSettingAsync(int id, CancellationToken cancellation = default);

    // UserLoanSize
    Task<Pager<VUserLoanSizes>> GetUserLoanSizesAsync(string? userRefId, QueryParam param, CancellationToken cancellation = default);
    Task<ErrorOr<UserLoanSizeDto>> GetUserLoanSizeByIdAsync(long id, CancellationToken cancellation = default);
    Task<ErrorOr<UserLoanSizeDto>> CreateUserLoanSizeAsync(UserLoanSizeDtoCreate request, CancellationToken cancellation = default);
    Task<ErrorOr<UserLoanSizeDto>> UpdateUserLoanSizeAsync(UserLoanSizeDtoUpdate request, CancellationToken cancellation = default);
    Task<ErrorOr<bool>> DeleteUserLoanSizeAsync(long? id, CancellationToken cancellation = default);
    Task<ErrorOr<bool>> ToggleUserLoanSizeActiveAsync(long id, string userRefId, CancellationToken cancellation = default);

    // CollateralSubType
    Task<Pager<CollateralSubTypeDto>> GetCollateralSubTypesAsync(QueryParam param, CancellationToken cancellation = default);
    Task<ErrorOr<VUserLoanSizes>> GetUserLoanSizeByUserAsync(string? userId, CancellationToken cancellation = default);
}
