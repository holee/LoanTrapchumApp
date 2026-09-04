using Application.Features.Settings.Dto;
using Application.Interfaces.Settings;
using ErrorOr;

namespace Application.Features.Settings.Commands.AppSettings.UpdateAppSetting;

public sealed class UpdateAppSettingCommandHandler(IAppSettingService repository)
    : IRequestHandler<UpdateAppSettingCommand, ErrorOr<AppSettingDto>>
{
    public Task<ErrorOr<AppSettingDto>> Handle(UpdateAppSettingCommand request, CancellationToken cancellationToken)
        => repository.UpdateAppSettingAsync(request.Request, cancellationToken);
}
