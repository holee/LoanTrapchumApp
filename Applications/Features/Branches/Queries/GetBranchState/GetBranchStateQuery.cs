using Applications.Responses;
using ErrorOr;

namespace Applications.Features.Branches.Queries.GetBranchState;

public record GetBranchStateQuery(string? UserId) : IRequest<ErrorOr<BranchStatus>> 
{
}
