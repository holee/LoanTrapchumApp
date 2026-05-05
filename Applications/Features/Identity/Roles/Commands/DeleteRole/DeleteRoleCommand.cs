using ErrorOr;

namespace Applications.Features.Identity.Roles.Commands.DeleteRole;

public sealed record DeleteRoleCommand(string RoleId) : IRequest<ErrorOr<Deleted>>;
