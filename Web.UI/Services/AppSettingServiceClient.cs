using Applications.Features.Settings.Dto;
using Applications.Interfaces.setting;
using Applications.Responses;
using Applications.Responses.Paginations;
using Domains.Models.Collaterals;
using Domains.Models.Loans;
using Domains.Models.Settings;

namespace Web.UI.Services;

/// <summary>
/// WebAssembly stand-in for the EF-backed <c>AppSettingService</c>.
/// Returns empty results so setting-driven components render without a server-side data context.
/// </summary>
public sealed class AppSettingServiceClient(HttpClient httpClient) : IAppSettingService
{
    public Task<Result<string>> Delete(long? id, CancellationToken cancellation = default) => Task.FromResult(Failed<string>());

    public Task<Result<string>> Save(AppSettings entity, CancellationToken cancellation = default) => Task.FromResult(Failed<string>());

    public Task<Result<string>> Update(AppSettings entity) => Task.FromResult(Failed<string>());

    public Task<Result<AppSettings>> GetSysSettingByCode(string code) => Task.FromResult(Result.Success(new AppSettings()));

    public Task<Result<AppSettings>> GetSysSettingByCode(int? id) => Task.FromResult(Result.Success(new AppSettings()));

    public Task<Result<IEnumerable<AppSettings>>> GetSysSettings(string? filter = null)
        => Task.FromResult(Result.Success<IEnumerable<AppSettings>>([]));

    public Task<Result<string>> DeActivateUserLoanSizeAsync(long? id, string? userRefId, CancellationToken cancellation = default)
        => Task.FromResult(Failed<string>());

    public Task<Result<string>> CreateUserLoanSizeAsync(UserLoanSize model, CancellationToken cancellation = default)
        => Task.FromResult(Failed<string>());

    public Task<Pager<VUserLoanSizes>> GetUserLoanSizes(string? userRefId, QueryParam q, CancellationToken cancellation = default)
        => Task.FromResult(new Pager<VUserLoanSizes>([]));

    public Task<Result<VUserLoanSizes>> GetLoanSizeByUserId(string? userRefId, CancellationToken cancellation = default)
        => Task.FromResult(Failed<VUserLoanSizes>());

    public Task<Result<UserLoanSize>> GetLoanSizeById(long? id, string? userId, CancellationToken cancellation = default)
        => Task.FromResult(Failed<UserLoanSize>());

    public Task<Result<string>> EditUserLoanSizeAsync(UserLoanSize model, CancellationToken cancellation = default)
        => Task.FromResult(Failed<string>());

    public Task<Pager<CollateralSubType>> GetCollateralSubTypes(QueryParam param, CancellationToken cancellation = default)
        => Task.FromResult(new Pager<CollateralSubType>([]));

    private static Result<T> Failed<T>() => Result.Failure<T>(default(T)!);
}
