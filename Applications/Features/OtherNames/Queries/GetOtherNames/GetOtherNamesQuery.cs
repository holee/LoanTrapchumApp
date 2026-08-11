using Applications.Interfaces.Lists;

namespace Applications.Features.OtherNames.Queries.GetOtherNames;

public record GetOtherNamesQuery(string? nameId, QueryParam param, params string[] columns) : IRequest<PageMetaData<VOtherNames>>;

public sealed class GetOtherNamesQueryHandler : IRequestHandler<GetOtherNamesQuery, PageMetaData<VOtherNames>>
{
    private readonly IOtherNameService _otherNameService;
    public GetOtherNamesQueryHandler(IOtherNameService otherNameService)
    {
        _otherNameService = otherNameService;
    }
    public async Task<PageMetaData<VOtherNames>> Handle(GetOtherNamesQuery request, CancellationToken cancellationToken)
    {

        var result = await _otherNameService.GetNameListsAsync(request.nameId, request.param, cancellationToken, request.columns);

        return result;
    }
}