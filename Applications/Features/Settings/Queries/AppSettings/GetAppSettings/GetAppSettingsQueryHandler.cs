using Applications.Features.Settings.Dto;
using Applications.Interfaces.Settings;
using ErrorOr;

namespace Applications.Features.Settings.Queries.AppSettings.GetAppSettings;

public sealed class GetAppSettingsQueryHandler(IAppSettingRepository repository)
    : IRequestHandler<GetAppSettingsQuery, ErrorOr<IEnumerable<AppSettingDto>>>
{
    public Task<ErrorOr<IEnumerable<AppSettingDto>>> Handle(GetAppSettingsQuery request, CancellationToken cancellationToken)
        => repository.GetAppSettingsAsync(request.Filter, cancellationToken);
}
