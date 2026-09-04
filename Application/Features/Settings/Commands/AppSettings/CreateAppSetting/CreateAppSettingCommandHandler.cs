using Application.Features.Settings.Dto;
using Application.Interfaces.Settings;
using ErrorOr;

namespace Application.Features.Settings.Commands.AppSettings.CreateAppSetting;

public sealed class CreateAppSettingCommandHandler(IAppSettingService repository)
    : IRequestHandler<CreateAppSettingCommand, ErrorOr<AppSettingDto>>
{
    public Task<ErrorOr<AppSettingDto>> Handle(CreateAppSettingCommand request, CancellationToken cancellationToken)
        => repository.CreateAppSettingAsync(request.Request, cancellationToken);
}
