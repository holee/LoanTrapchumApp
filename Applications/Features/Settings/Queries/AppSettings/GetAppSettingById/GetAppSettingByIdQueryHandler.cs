using Applications.Features.Settings.Dto;
using Applications.Interfaces.Settings;
using ErrorOr;

namespace Applications.Features.Settings.Queries.AppSettings.GetAppSettingById;

public sealed class GetAppSettingByIdQueryHandler(IAppSettingService repository)
    : IRequestHandler<GetAppSettingByIdQuery, ErrorOr<AppSettingDto>>
{
    public Task<ErrorOr<AppSettingDto>> Handle(GetAppSettingByIdQuery request, CancellationToken cancellationToken)
        => repository.GetAppSettingByIdAsync(request.Id, cancellationToken);
}
