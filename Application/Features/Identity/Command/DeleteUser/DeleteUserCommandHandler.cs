using Application.Interfaces.Identity;
using ErrorOr;

namespace Application.Features.Identity.Command.DeleteUser;

public sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, ErrorOr<DeleteResult>>
{
    private readonly IUserService _userService;
    public DeleteUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<ErrorOr<DeleteResult>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        return await _userService.DeleteAsync(request.Id, cancellationToken);
    }
}