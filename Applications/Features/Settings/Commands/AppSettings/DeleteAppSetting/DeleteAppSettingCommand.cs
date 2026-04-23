using ErrorOr;

namespace Applications.Features.Settings.Commands.AppSettings.DeleteAppSetting;

public record DeleteAppSettingCommand(int Id) : IRequest<ErrorOr<bool>>;
