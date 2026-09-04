using Application.Features.Branches.Dtos;
using ErrorOr;

namespace Application.Features.Branches.Queries.GetBranchForVault;

public record GetBranchForVaultQuery(int? BranchId) : IRequest<ErrorOr<VBranch>>
{
}
