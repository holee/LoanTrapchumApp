using Application.Interfaces.Lists;

namespace Application.Features.OtherNames.Queries.GetNameAsPaging;

public sealed class GetNameAsPagingQueryHandler(IOtherNameService otherNameService) : IRequestHandler<GetNameAsPagingQuery, Pager<VOtherNames>>
{
    public async Task<Pager<VOtherNames>> Handle(GetNameAsPagingQuery request, CancellationToken cancellationToken)
    {
        return await otherNameService.GetNameAsync(request.NameId, request.Param, cancellationToken);
    }
}
