using Application.Interfaces.Identity;
using Application.Responses.ViewModels.Auths;
using ErrorOr;

namespace Application.Features.Identity.Queries.GetUserDetailById;

public sealed class GetUserDetailsByIdQueryHandler : IRequestHandler<GetUserDetailsByIdQuery, ErrorOr<VUser>>
{
    private readonly IUserService _identityService;
    public GetUserDetailsByIdQueryHandler(IUserService identityService)
    {
        _identityService = identityService;
    }
    public async Task<ErrorOr<VUser>> Handle(GetUserDetailsByIdQuery request, CancellationToken cancellationToken)
    {
        return await _identityService.GetUserDetailById(request.UserId);

    }
}
