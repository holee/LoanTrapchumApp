using Applications.Features.Identity.Roles.Responses;

namespace Applications.Features.Identity.Roles.Queries.GetRolesAsPaging;

public sealed record GetRolesAsPagingQuery(QueryParam Param) : IRequest<Pager<RoleDto>>;
