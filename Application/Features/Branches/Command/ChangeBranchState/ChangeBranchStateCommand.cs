using Application.Features.Branches.Dtos;
using ErrorOr;

namespace Application.Features.Branches.Command.ChangeBranchState;

public record ChangeBranchStateCommand(int? BranchId,bool State) : IRequest<ErrorOr<VBranch>>;
