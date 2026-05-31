using Applications.Features.Identity.Users;
using ErrorOr;

namespace Applications.Features.Identity.Queries.GetUserById;

public record GetUserByIdQuery(string Id) : IRequest<ErrorOr<UserDto>>;
