using Applications.Interfaces.Lists;

namespace Applications.Features.OtherNames.Queries.GetNameAsPaging;

public sealed class GetNameAsPagingQueryHandler<T>(IOtherNameService otherNameService)
    : IRequestHandler<GetNameAsPagingQuery<T>, Pager<T>> where T : class, new()
{
    public async Task<Pager<T>> Handle(GetNameAsPagingQuery<T> request, CancellationToken cancellationToken)
    {
        return await otherNameService.GetNameAsync<T>(request.NameId, request.Param, cancellationToken);
    }
}
