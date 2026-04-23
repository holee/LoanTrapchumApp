using Applications.Interfaces.Settings;
using ErrorOr;

namespace Applications.Features.Settings.Commands.AppSettings.DeleteAppSetting;

public sealed class DeleteAppSettingCommandHandler(IAppSettingRepository repository)
    : IRequestHandler<DeleteAppSettingCommand, ErrorOr<bool>>
{
    public Task<ErrorOr<bool>> Handle(DeleteAppSettingCommand request, CancellationToken cancellationToken)
        => repository.DeleteAppSettingAsync(request.Id, cancellationToken);
}
