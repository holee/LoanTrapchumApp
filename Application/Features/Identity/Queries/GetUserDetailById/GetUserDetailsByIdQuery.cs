using Application.Responses.ViewModels.Auths;
using ErrorOr;

namespace Application.Features.Identity.Queries.GetUserDetailById;

public record GetUserDetailsByIdQuery(string? UserId) : IRequest<ErrorOr<VUser>>;
