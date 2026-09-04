using Application.Features.Identity.Users;
using ErrorOr;

namespace Application.Features.Identity.Roles.Commands.AddRolePermissions;

public sealed record AddRolePermissionsCommand(RolePermissionRequest Permission) : IRequest<ErrorOr<Success>>;
