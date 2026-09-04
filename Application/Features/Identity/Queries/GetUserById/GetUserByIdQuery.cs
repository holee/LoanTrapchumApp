using Application.Features.Identity.Users;
using ErrorOr;

namespace Application.Features.Identity.Queries.GetUserById;

public record GetUserByIdQuery(string Id) : IRequest<ErrorOr<UserDto>>;
