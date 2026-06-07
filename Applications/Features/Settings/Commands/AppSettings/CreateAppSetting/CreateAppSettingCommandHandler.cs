using Applications.Features.Settings.Dto;
using Applications.Interfaces.Settings;
using ErrorOr;

namespace Applications.Features.Settings.Commands.AppSettings.CreateAppSetting;

public sealed class CreateAppSettingCommandHandler(IAppSettingService repository)
    : IRequestHandler<CreateAppSettingCommand, ErrorOr<AppSettingDto>>
{
    public Task<ErrorOr<AppSettingDto>> Handle(CreateAppSettingCommand request, CancellationToken cancellationToken)
        => repository.CreateAppSettingAsync(request.Request, cancellationToken);
}
