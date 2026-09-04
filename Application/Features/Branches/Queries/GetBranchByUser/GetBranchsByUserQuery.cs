using ErrorOr;

namespace Application.Features.Branches.Queries.GetBranchByUser;

public record GetBranchByUserQuery(string? UserId) : IRequest<ErrorOr<VBranch>>;
