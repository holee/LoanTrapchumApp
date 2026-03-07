namespace Applications.Features.Identity.Roles.Queries.GetRoles;

public record GetRolesQuery(string? Filter) : IRequest<IEnumerable<string>>;
