using Applications.Features.Settings.Dto;
using Applications.Interfaces.Settings;
using ErrorOr;

namespace Applications.Features.Settings.Commands.AppSettings.UpdateAppSetting;

public sealed class UpdateAppSettingCommandHandler(IAppSettingRepository repository)
    : IRequestHandler<UpdateAppSettingCommand, ErrorOr<AppSettingDto>>
{
    public Task<ErrorOr<AppSettingDto>> Handle(UpdateAppSettingCommand request, CancellationToken cancellationToken)
        => repository.UpdateAppSettingAsync(request.Request, cancellationToken);
}
