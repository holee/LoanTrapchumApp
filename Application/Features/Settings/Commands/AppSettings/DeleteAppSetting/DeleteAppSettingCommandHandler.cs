using Application.Interfaces.Settings;
using ErrorOr;

namespace Application.Features.Settings.Commands.AppSettings.DeleteAppSetting;

public sealed class DeleteAppSettingCommandHandler(IAppSettingService repository)
    : IRequestHandler<DeleteAppSettingCommand, ErrorOr<bool>>
{
    public Task<ErrorOr<bool>> Handle(DeleteAppSettingCommand request, CancellationToken cancellationToken)
        => repository.DeleteAppSettingAsync(request.Id, cancellationToken);
}
