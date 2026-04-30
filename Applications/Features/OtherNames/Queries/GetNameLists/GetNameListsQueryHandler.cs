using Applications.Interfaces.Lists;

namespace Applications.Features.OtherNames.Queries.GetNameLists;

public sealed class GetNameListsQueryHandler<T>(IOtherNameService otherNameService)
    : IRequestHandler<GetNameListsQuery<T>, IEnumerable<T>> where T : class, new()
{
    public async Task<IEnumerable<T>> Handle(GetNameListsQuery<T> request, CancellationToken cancellationToken)
    {
        return await otherNameService.GetNameListsAsync<T>(request.NameId, request.Param, cancellationToken);
    }
}
