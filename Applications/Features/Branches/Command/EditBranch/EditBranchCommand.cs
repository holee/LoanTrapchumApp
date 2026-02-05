using Applications.Features.Branches.Dtos;
using ErrorOr;

namespace Applications.Features.Branches.Command.EditBranch; 

public record EditBranchCommand(BranchDtoUpdate Branch) : IRequest<ErrorOr<VBranch>>;