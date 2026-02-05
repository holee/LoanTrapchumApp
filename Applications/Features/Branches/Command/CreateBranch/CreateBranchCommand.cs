using Applications.Features.Branches.Dtos;
using ErrorOr;

namespace Applications.Features.Branches.Command.CreateBranch;

public record CreateBranchCommand(BranchDtoCreate Branch) : IRequest<ErrorOr<VBranch>>;
