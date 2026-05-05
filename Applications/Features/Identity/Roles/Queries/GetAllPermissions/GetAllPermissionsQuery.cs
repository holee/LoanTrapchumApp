using Applications.Responses.ViewModels.Auths;

namespace Applications.Features.Identity.Roles.Queries.GetAllPermissions;

public sealed record GetAllPermissionsQuery() : IRequest<List<PermissionsDtoCreate>>;
