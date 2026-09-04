using Application.Features.Identity.Users;
using ErrorOr;

namespace Application.Features.Identity.Command.UpdateUserRole;

public record UpdateUserRoleCommand(UserRoleDtoUpdate Request) : IRequest<ErrorOr<Updated>>;
