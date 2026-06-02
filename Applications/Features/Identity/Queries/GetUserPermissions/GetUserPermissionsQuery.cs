using Applications.Responses.Auths;
using ErrorOr;

namespace Applications.Features.Identity.Queries.GetUserPermissions;

public sealed record GetUserPermissionsQuery(string? UserId) : IRequest<ErrorOr<UserPermissions>>;
