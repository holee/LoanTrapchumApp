using Application.Features.Settings.Dto;
using Application.Interfaces.Settings;
using ErrorOr;

namespace Application.Features.Settings.Queries.UserLoanSizes.GetLoanSizeByUser;

public sealed class GetLoanSizeByUserQueryHandler(IAppSettingService repository)
    : IRequestHandler<GetLoanSizeByUserQuery, ErrorOr<VUserLoanSizes>>
{
    public Task<ErrorOr<VUserLoanSizes>> Handle(GetLoanSizeByUserQuery request, CancellationToken cancellationToken)
    {
        return repository.GetUserLoanSizeByUserAsync(request.UserId, cancellationToken);
    }

}
