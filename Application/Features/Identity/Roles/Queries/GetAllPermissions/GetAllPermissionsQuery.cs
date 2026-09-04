using Application.Responses.ViewModels.Auths;

namespace Application.Features.Identity.Roles.Queries.GetAllPermissions;

public sealed record GetAllPermissionsQuery() : IRequest<List<PermissionsDtoCreate>>;
