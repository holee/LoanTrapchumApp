using Application.Features.Branches.Dtos;
using ErrorOr;

namespace Application.Features.Branches.Queries.GetUpdateBranch;

public record GetUpdateBranchQuery(int? BranchId) : IRequest<ErrorOr<BranchDtoUpdate>>;
