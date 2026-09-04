using Application.Responses.Auths;
using ErrorOr;

namespace Application.Features.Identity.Roles.Queries.GetPermissionsByRole;

public sealed record GetPermissionsByRoleQuery(string RoleId) : IRequest<ErrorOr<RolePermissions>>;
