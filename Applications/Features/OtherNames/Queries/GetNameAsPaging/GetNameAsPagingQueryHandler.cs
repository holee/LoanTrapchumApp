using Applications.Interfaces.Lists;

namespace Applications.Features.OtherNames.Queries.GetNameAsPaging;

public sealed class GetNameAsPagingQueryHandler(IOtherNameService otherNameService) : IRequestHandler<GetNameAsPagingQuery, Pager<VOtherNames>>
{
    public async Task<Pager<VOtherNames>> Handle(GetNameAsPagingQuery request, CancellationToken cancellationToken)
    {
        return await otherNameService.GetNameAsync(request.NameId, request.Param, cancellationToken);
    }
}
