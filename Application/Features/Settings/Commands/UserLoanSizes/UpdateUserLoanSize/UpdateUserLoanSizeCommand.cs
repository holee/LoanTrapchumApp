using Application.Features.Settings.Dto;
using ErrorOr;

namespace Application.Features.Settings.Commands.UserLoanSizes.UpdateUserLoanSize;

public record UpdateUserLoanSizeCommand(UserLoanSizeDtoUpdate Request) : IRequest<ErrorOr<UserLoanSizeDto>>;
