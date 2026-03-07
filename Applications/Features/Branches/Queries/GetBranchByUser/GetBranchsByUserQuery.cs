using ErrorOr;

namespace Applications.Features.Branches.Queries.GetBranchByUser;

public record GetBranchByUserQuery(string? UserId) : IRequest<ErrorOr<VBranch>>;
