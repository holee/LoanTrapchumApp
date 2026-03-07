using ErrorOr;

namespace Applications.Features.Branches.Queries.GetBranchsByUser;

public record GetBranchsByUserQuery(string? UserId, int? BranchId) : IRequest<ErrorOr<IEnumerable<VBranch>>>;
