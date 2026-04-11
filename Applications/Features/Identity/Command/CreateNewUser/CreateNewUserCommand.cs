using Applications.Features.Identity.Users;
using ErrorOr;

namespace Applications.Features.Identity.Command.CreateNewUser;

public sealed record CreateNewUserCommand(RegisterRequest Request) : IRequest<ErrorOr<string>>;
