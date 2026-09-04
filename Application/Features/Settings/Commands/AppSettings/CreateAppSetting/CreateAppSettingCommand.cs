using Application.Features.Settings.Dto;
using ErrorOr;

namespace Application.Features.Settings.Commands.AppSettings.CreateAppSetting;

public record CreateAppSettingCommand(AppSettingDtoCreate Request) : IRequest<ErrorOr<AppSettingDto>>;
