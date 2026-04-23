using Applications.Features.Settings.Dto;
using Applications.Interfaces.Settings;
using ErrorOr;

namespace Applications.Features.Settings.Queries.UserLoanSizes.GetUserLoanSizeById;

public sealed class GetUserLoanSizeByIdQueryHandler(IAppSettingRepository repository)
    : IRequestHandler<GetUserLoanSizeByIdQuery, ErrorOr<UserLoanSizeDto>>
{
    public Task<ErrorOr<UserLoanSizeDto>> Handle(GetUserLoanSizeByIdQuery request, CancellationToken cancellationToken)
        => repository.GetUserLoanSizeByIdAsync(request.Id, cancellationToken);
}
