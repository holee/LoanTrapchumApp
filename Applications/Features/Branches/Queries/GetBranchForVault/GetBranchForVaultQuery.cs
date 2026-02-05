using Applications.Features.Branches.Dtos;
using ErrorOr;

namespace Applications.Features.Branches.Queries.GetBranchForVault;

public record GetBranchForVaultQuery(int? BranchId) : IRequest<ErrorOr<VBranch>>
{
}
