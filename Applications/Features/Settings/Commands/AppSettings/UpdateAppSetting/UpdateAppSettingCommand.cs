using Applications.Features.Settings.Dto;
using ErrorOr;

namespace Applications.Features.Settings.Commands.AppSettings.UpdateAppSetting;

public record UpdateAppSettingCommand(AppSettingDtoUpdate Request) : IRequest<ErrorOr<AppSettingDto>>;
