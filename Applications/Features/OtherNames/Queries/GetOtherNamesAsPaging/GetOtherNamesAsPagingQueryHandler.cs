using Applications.Interfaces.Lists;
using Applications.Responses.ViewModels.OtherNames;

namespace Applications.Features.OtherNames.Queries.GetOtherNamesAsPaging;

public sealed class GetOtherNamesAsPagingQueryHandler(IOtherNameService otherNameService)
    : IRequestHandler<GetOtherNamesAsPagingQuery, Pager<VOtherNames>>
{
    public async Task<Pager<VOtherNames>> Handle(GetOtherNamesAsPagingQuery request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(request.Type))
            return await otherNameService.GetNamesAsync(request.Param, request.Type, cancellationToken);

        return await otherNameService.GetNamesAsync(request.Param, cancellationToken);
    }
}
