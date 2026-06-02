using Applications.Features.Identity.Users;
using Applications.Interfaces.Identity;
using ErrorOr;

namespace Applications.Features.Identity.Queries.GetUserById;

public sealed class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ErrorOr<UserDto>>
{
    private readonly IUserService _userService;
    public GetUserByIdQueryHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<ErrorOr<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _userService.GetUserDetailsById(request.Id);

        return result;
    }
}