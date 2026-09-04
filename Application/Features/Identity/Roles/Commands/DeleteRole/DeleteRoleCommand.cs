using ErrorOr;

namespace Application.Features.Identity.Roles.Commands.DeleteRole;

public sealed record DeleteRoleCommand(string RoleId) : IRequest<ErrorOr<Deleted>>;
