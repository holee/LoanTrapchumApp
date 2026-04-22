using Applications.Features.Settings.Dto;
using ErrorOr;

namespace Applications.Features.Settings.Commands.UserLoanSizes.UpdateUserLoanSize;

public record UpdateUserLoanSizeCommand(UserLoanSizeDtoUpdate Request) : IRequest<ErrorOr<UserLoanSizeDto>>;
