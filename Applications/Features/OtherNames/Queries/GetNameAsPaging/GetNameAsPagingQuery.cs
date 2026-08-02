namespace Applications.Features.OtherNames.Queries.GetNameAsPaging;

public record GetNameAsPagingQuery(string? NameId, QueryParam Param) : IRequest<Pager<VOtherNames>>;
