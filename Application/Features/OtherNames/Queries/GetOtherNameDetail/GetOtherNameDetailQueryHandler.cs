using Application.Interfaces.Lists;
using ErrorOr;

namespace Application.Features.OtherNames.Queries.GetOtherNameDetail;

public sealed class GetOtherNameDetailQueryHandler(IOtherNameService otherNameService) : IRequestHandler<GetOtherNameDetailQuery, ErrorOr<VOtherNames>>
{
    public async Task<ErrorOr<VOtherNames>> Handle(GetOtherNameDetailQuery request, CancellationToken cancellationToken)
    {
        return await otherNameService.GetDetailAsync(request.Id, cancellationToken);
    }
}
