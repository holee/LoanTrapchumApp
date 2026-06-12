using Applications.Interfaces.Settings;
namespace Applications.Features.Settings.Queries.GetSystemSettings;

public class GetSystemSettingQueryHandler(IAppSettingService settingService) : IRequestHandler<GetSystemSettingQuery, Pager<Domains.Models.Settings.AppSettings>>
{
    public async Task<Pager<Domains.Models.Settings.AppSettings>> Handle(GetSystemSettingQuery request, CancellationToken cancellationToken)
    {
        var settings = await settingService.GetSystemSettings(request.Id, request.Param);

        return settings;
    }
}
