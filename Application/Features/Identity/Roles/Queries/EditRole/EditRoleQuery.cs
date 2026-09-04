using Application.Features.Identity.Roles.Requests;
using ErrorOr;

namespace Application.Features.Identity.Roles.Queries.EditRole;

public sealed record EditRoleQuery(string RoleId) : IRequest<ErrorOr<RoleDtoUpdate>>;
