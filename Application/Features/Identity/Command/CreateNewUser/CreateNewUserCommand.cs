using Application.Features.Identity.Users;
using ErrorOr;

namespace Application.Features.Identity.Command.CreateNewUser;

public sealed record CreateNewUserCommand(RegisterRequest Request) : IRequest<ErrorOr<string>>;
