using Applications.Features.Identity.Roles.Requests;
using ErrorOr;

namespace Applications.Features.Identity.Roles.Commands.UpdateRole;

public sealed record UpdateRoleCommand(RoleDtoUpdate Role) : IRequest<ErrorOr<string>>;
