using Applications.Features.Settings.Queries.GetSystemSettings;
using Applications.Utils;
using Domains.Models.Settings;

namespace TrapchumApp.Services;

public sealed class AppSettingService(IMediator mediator)
{
    public async Task<Pager<AppSettings>> GetSystemSettings(int? id = 0, QueryParam? param = null)
    {

        param ??= new QueryParam();

        var command = new GetSystemSettingQuery(id, param);

        return await mediator.Send(command);
    }
}
