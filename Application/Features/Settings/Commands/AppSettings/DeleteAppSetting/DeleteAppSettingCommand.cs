using ErrorOr;

namespace Application.Features.Settings.Commands.AppSettings.DeleteAppSetting;

public record DeleteAppSettingCommand(int Id) : IRequest<ErrorOr<bool>>;
