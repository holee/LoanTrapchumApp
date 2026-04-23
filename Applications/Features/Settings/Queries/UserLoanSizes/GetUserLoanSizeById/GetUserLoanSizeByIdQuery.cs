using Applications.Features.Settings.Dto;
using ErrorOr;

namespace Applications.Features.Settings.Queries.UserLoanSizes.GetUserLoanSizeById;

public record GetUserLoanSizeByIdQuery(long Id) : IRequest<ErrorOr<UserLoanSizeDto>>;
