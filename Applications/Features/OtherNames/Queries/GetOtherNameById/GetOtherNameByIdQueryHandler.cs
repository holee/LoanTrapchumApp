using Applications.Interfaces.Lists;
using Domains.Models.Lists;
using ErrorOr;

namespace Applications.Features.OtherNames.Queries.GetOtherNameById;

public sealed class GetOtherNameByIdQueryHandler(IOtherNameService otherNameService)
    : IRequestHandler<GetOtherNameByIdQuery, ErrorOr<EntityNames>>
{
    public async Task<ErrorOr<EntityNames>> Handle(GetOtherNameByIdQuery request, CancellationToken cancellationToken)
    {
        return await otherNameService.GetByIdAsync(request.Id, cancellationToken);
    }
}
