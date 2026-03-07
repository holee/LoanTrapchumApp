using Applications.Responses.ViewModels.Auths;
using ErrorOr;

namespace Applications.Features.Identity.Queries.GetUserDetailById;

public record GetUserDetailsByIdQuery(string? UserId) : IRequest<ErrorOr<VUser>>;
