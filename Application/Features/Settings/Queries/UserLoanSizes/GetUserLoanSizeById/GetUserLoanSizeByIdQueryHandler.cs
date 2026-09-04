using Application.Features.Settings.Dto;
using Application.Interfaces.Settings;
using ErrorOr;

namespace Application.Features.Settings.Queries.UserLoanSizes.GetUserLoanSizeById;

public sealed class GetUserLoanSizeByIdQueryHandler(IAppSettingService repository)
    : IRequestHandler<GetUserLoanSizeByIdQuery, ErrorOr<UserLoanSizeDto>>
{
    public Task<ErrorOr<UserLoanSizeDto>> Handle(GetUserLoanSizeByIdQuery request, CancellationToken cancellationToken)
        => repository.GetUserLoanSizeByIdAsync(request.Id, cancellationToken);
}
