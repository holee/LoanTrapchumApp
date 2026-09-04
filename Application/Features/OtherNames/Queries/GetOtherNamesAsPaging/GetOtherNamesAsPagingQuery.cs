using Application.Responses.ViewModels.OtherNames;

namespace Application.Features.OtherNames.Queries.GetOtherNamesAsPaging;

public record GetOtherNamesAsPagingQuery(QueryParam Param, string? Type = null) : IRequest<Pager<VOtherNames>>;
