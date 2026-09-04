using Application.Responses.Auths;
using ErrorOr;

namespace Application.Features.Identity.Queries.GetUserPermissions;

public sealed record GetUserPermissionsQuery(string? UserId) : IRequest<ErrorOr<UserPermissions>>;
