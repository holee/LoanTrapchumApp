using Application.Features.Settings.Dto;
using Application.Interfaces.Settings;
using ErrorOr;

namespace Application.Features.Settings.Queries.AppSettings.GetAppSettings;

public sealed class GetAppSettingsQueryHandler(IAppSettingService repository)
    : IRequestHandler<GetAppSettingsQuery, ErrorOr<IEnumerable<AppSettingDto>>>
{
    public Task<ErrorOr<IEnumerable<AppSettingDto>>> Handle(GetAppSettingsQuery request, CancellationToken cancellationToken)
        => repository.GetAppSettingsAsync(request.Filter, cancellationToken);
}
