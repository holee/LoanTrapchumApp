using Application.Features.Settings.Dto;
using ErrorOr;

namespace Application.Features.Settings.Commands.UserLoanSizes.CreateUserLoanSize;

public record CreateUserLoanSizeCommand(UserLoanSizeDtoCreate Request) : IRequest<ErrorOr<UserLoanSizeDto>>;
