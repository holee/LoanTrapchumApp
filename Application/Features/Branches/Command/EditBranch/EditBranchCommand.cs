using Application.Features.Branches.Dtos;
using ErrorOr;

namespace Application.Features.Branches.Command.EditBranch; 

public record EditBranchCommand(BranchDtoUpdate Branch) : IRequest<ErrorOr<VBranch>>;