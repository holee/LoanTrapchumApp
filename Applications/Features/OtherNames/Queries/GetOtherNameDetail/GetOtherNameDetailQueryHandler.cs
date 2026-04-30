using Applications.Interfaces.Lists;
using ErrorOr;

namespace Applications.Features.OtherNames.Queries.GetOtherNameDetail;

public sealed class GetOtherNameDetailQueryHandler<T>(IOtherNameService otherNameService)
    : IRequestHandler<GetOtherNameDetailQuery<T>, ErrorOr<T>> where T : class
{
    public async Task<ErrorOr<T>> Handle(GetOtherNameDetailQuery<T> request, CancellationToken cancellationToken)
    {
        return await otherNameService.GetDetailAsync<T>(request.Id, cancellationToken);
    }
}
