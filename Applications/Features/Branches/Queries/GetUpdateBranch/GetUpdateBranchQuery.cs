using Applications.Features.Branches.Dtos;
using ErrorOr;

namespace Applications.Features.Branches.Queries.GetUpdateBranch;

public record GetUpdateBranchQuery(int? BranchId) : IRequest<ErrorOr<BranchDtoUpdate>>;
