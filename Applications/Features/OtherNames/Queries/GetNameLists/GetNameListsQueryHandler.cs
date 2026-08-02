using Applications.Interfaces.Lists;

namespace Applications.Features.OtherNames.Queries.GetNameLists;

public sealed class GetNameListsQueryHandler(IOtherNameService otherNameService) : IRequestHandler<GetNameListsQuery, IEnumerable<VOtherNames>>
{
    public async Task<IEnumerable<VOtherNames>> Handle(GetNameListsQuery request, CancellationToken cancellationToken)
    {
        return await otherNameService.GetNameListsAsync(request.NameId, request.Param, cancellationToken);
    }
}
