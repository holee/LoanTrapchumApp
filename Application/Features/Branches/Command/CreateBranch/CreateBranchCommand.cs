using Application.Features.Branches.Dtos;
using ErrorOr;

namespace Application.Features.Branches.Command.CreateBranch;

public record CreateBranchCommand(BranchDtoCreate Branch) : IRequest<ErrorOr<VBranch>>;
