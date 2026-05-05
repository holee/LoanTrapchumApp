using Applications.Features.Identity.Roles.Requests;
using ErrorOr;

namespace Applications.Features.Identity.Roles.Queries.EditRole;

public sealed record EditRoleQuery(string RoleId) : IRequest<ErrorOr<RoleDtoUpdate>>;
