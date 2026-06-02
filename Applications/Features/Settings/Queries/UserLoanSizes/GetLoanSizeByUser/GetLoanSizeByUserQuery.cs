using Applications.Features.Settings.Dto;
using ErrorOr;

namespace Applications.Features.Settings.Queries.UserLoanSizes.GetLoanSizeByUser;

public record GetLoanSizeByUserQuery(string? UserId) : IRequest<ErrorOr<VUserLoanSizes>>;
