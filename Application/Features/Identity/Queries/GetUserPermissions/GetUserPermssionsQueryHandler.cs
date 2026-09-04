using Application.Interfaces.Identity;
using Application.Responses.Auths;
using ErrorOr;

namespace Application.Features.Identity.Queries.GetUserPermissions;

public sealed class GetUserPermssionsQueryHandler : IRequestHandler<GetUserPermissionsQuery, ErrorOr<UserPermissions>>
{
    private readonly IUserService _userService;
    public GetUserPermssionsQueryHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<ErrorOr<UserPermissions>> Handle(GetUserPermissionsQuery request, CancellationToken cancellationToken)
    {

        var result = await _userService.GetUserPermissionsAsync(request.UserId);

        return result;
    }
}