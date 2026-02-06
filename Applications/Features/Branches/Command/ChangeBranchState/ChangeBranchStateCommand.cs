using Applications.Features.Branches.Dtos;
using ErrorOr;

namespace Applications.Features.Branches.Command.ChangeBranchState;

public record ChangeBranchStateCommand(int? BranchId,bool State) : IRequest<ErrorOr<VBranch>>;
