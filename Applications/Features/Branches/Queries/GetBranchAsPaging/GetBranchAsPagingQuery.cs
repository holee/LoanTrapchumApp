using Applications.Features.Branches.Dtos;
using ErrorOr;

namespace Applications.Features.Branches.Queries.GetBranchAsPaging;

public record GetBranchAsPagingQuery(int? BranchId, QueryParam Param) : IRequest<Pager<VBranch>>;
