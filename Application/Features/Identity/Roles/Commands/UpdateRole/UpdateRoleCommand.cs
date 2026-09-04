using Application.Features.Identity.Roles.Requests;
using ErrorOr;

namespace Application.Features.Identity.Roles.Commands.UpdateRole;

public sealed record UpdateRoleCommand(RoleDtoUpdate Role) : IRequest<ErrorOr<string>>;
