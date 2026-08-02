namespace Applications.Features.OtherNames.Queries.GetNameLists;

public record GetNameListsQuery(string? NameId, QueryParam Param) : IRequest<IEnumerable<VOtherNames>>;