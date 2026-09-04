using Application.Responses;
using ErrorOr;

namespace Application.Features.Branches.Queries.GetBranchState;

public record GetBranchStateQuery(string? UserId) : IRequest<ErrorOr<BranchStatus>> 
{
}
