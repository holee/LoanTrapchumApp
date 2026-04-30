namespace Applications.Features.OtherNames.Queries.GetNameLists;

public record GetNameListsQuery<T>(string? NameId, QueryParam Param) : IRequest<IEnumerable<T>> where T : class, new();
