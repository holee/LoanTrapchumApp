using Application.Features.Settings.Dto;
using ErrorOr;

namespace Application.Features.Settings.Commands.AppSettings.UpdateAppSetting;

public record UpdateAppSettingCommand(AppSettingDtoUpdate Request) : IRequest<ErrorOr<AppSettingDto>>;
