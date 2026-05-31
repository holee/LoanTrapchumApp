using Applications.Features.Identity.Users;
using ErrorOr;

namespace Applications.Features.Identity.Command.UpdateUserRole;

public record UpdateUserRoleCommand(UserRoleDtoUpdate Request) : IRequest<ErrorOr<Updated>>;
