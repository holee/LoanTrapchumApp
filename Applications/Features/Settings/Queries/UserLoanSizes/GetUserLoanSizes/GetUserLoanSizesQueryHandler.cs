using Applications.Features.Settings.Dto;
using Applications.Interfaces.Settings;
using Applications.Responses.Paginations;
using Applications.Responses.ViewModels.Settings;

namespace Applications.Features.Settings.Queries.UserLoanSizes.GetUserLoanSizes;

public sealed class GetUserLoanSizesQueryHandler(IAppSettingRepository repository)
    : IRequestHandler<GetUserLoanSizesQuery, Pager<VUserLoanSizes>>
{
    public Task<Pager<VUserLoanSizes>> Handle(GetUserLoanSizesQuery request, CancellationToken cancellationToken)
        => repository.GetUserLoanSizesAsync(request.UserRefId, request.Param, cancellationToken);
}
