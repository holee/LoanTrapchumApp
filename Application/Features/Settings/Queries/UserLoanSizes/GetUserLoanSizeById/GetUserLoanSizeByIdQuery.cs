using Application.Features.Settings.Dto;
using ErrorOr;

namespace Application.Features.Settings.Queries.UserLoanSizes.GetUserLoanSizeById;

public record GetUserLoanSizeByIdQuery(long Id) : IRequest<ErrorOr<UserLoanSizeDto>>;
