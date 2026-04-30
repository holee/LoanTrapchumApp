namespace Applications.Features.OtherNames.Queries.GetNameAsPaging;

public record GetNameAsPagingQuery<T>(string? NameId, QueryParam Param) : IRequest<Pager<T>> where T : class, new();
