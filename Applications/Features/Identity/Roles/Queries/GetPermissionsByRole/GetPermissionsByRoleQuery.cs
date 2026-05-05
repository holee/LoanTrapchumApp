using Applications.Responses.Auths;
using ErrorOr;

namespace Applications.Features.Identity.Roles.Queries.GetPermissionsByRole;

public sealed record GetPermissionsByRoleQuery(string RoleId) : IRequest<ErrorOr<RolePermissions>>;
