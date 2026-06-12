using Applications.Interfaces.Settings;
using Applications.Responses.Paginations;

namespace Applications.Features.Settings.Queries.UserLoanSizes.GetUserLoanSizes;

public sealed class GetUserLoanSizesQueryHandler(IAppSettingService repository)
    : IRequestHandler<GetUserLoanSizesQuery, Pager<VUserLoanSizes>>
{
    public Task<Pager<VUserLoanSizes>> Handle(GetUserLoanSizesQuery request, CancellationToken cancellationToken)
        => repository.GetUserLoanSizesAsync(request.UserRefId, request.Param, cancellationToken);
}
