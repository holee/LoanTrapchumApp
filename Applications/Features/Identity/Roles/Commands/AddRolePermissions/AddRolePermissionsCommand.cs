using Applications.Features.Identity.Users;
using ErrorOr;

namespace Applications.Features.Identity.Roles.Commands.AddRolePermissions;

public sealed record AddRolePermissionsCommand(RolePermissionRequest Permission) : IRequest<ErrorOr<Success>>;
