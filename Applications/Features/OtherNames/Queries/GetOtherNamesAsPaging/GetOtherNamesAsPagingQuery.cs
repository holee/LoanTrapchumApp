using Applications.Responses.ViewModels.OtherNames;

namespace Applications.Features.OtherNames.Queries.GetOtherNamesAsPaging;

public record GetOtherNamesAsPagingQuery(QueryParam Param, string? Type = null) : IRequest<Pager<VOtherNames>>;
