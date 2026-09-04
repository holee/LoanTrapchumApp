using Application.Features.Branches.Dtos;
using ErrorOr;

namespace Application.Features.Branches.Queries.GetBranchAsPaging;

public record GetBranchAsPagingQuery(int? BranchId, QueryParam Param) : IRequest<Pager<VBranch>>;
