using Application.Features.Identity.Roles.Responses;

namespace Application.Features.Identity.Roles.Queries.GetRolesAsPaging;

public sealed record GetRolesAsPagingQuery(QueryParam Param) : IRequest<Pager<RoleDto>>;
