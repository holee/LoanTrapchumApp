using Applications.Features.Settings.Dto;
using ErrorOr;

namespace Applications.Features.Settings.Commands.AppSettings.CreateAppSetting;

public record CreateAppSettingCommand(AppSettingDtoCreate Request) : IRequest<ErrorOr<AppSettingDto>>;
