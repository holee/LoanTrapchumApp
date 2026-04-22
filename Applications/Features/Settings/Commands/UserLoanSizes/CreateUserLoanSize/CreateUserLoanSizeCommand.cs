using Applications.Features.Settings.Dto;
using ErrorOr;

namespace Applications.Features.Settings.Commands.UserLoanSizes.CreateUserLoanSize;

public record CreateUserLoanSizeCommand(UserLoanSizeDtoCreate Request) : IRequest<ErrorOr<UserLoanSizeDto>>;
