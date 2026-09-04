using Application.Features.Settings.Dto;
using ErrorOr;

namespace Application.Features.Settings.Queries.UserLoanSizes.GetLoanSizeByUser;

public record GetLoanSizeByUserQuery(string? UserId) : IRequest<ErrorOr<VUserLoanSizes>>;
