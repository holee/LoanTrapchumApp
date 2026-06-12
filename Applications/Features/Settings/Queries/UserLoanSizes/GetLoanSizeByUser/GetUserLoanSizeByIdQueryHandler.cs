using Applications.Features.Settings.Dto;
using Applications.Interfaces.Settings;
using ErrorOr;

namespace Applications.Features.Settings.Queries.UserLoanSizes.GetLoanSizeByUser;

public sealed class GetLoanSizeByUserQueryHandler(IAppSettingService repository)
    : IRequestHandler<GetLoanSizeByUserQuery, ErrorOr<VUserLoanSizes>>
{
    public Task<ErrorOr<VUserLoanSizes>> Handle(GetLoanSizeByUserQuery request, CancellationToken cancellationToken)
    {
        return repository.GetUserLoanSizeByUserAsync(request.UserId, cancellationToken);
    }

}
