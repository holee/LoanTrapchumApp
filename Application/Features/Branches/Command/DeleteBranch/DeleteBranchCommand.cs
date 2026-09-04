using ErrorOr;

namespace Application.Features.Branches.Command.DeleteBranch;

public record DeleteBranchCommand(int? BranchId) : IRequest<ErrorOr<DeleteResult>>;
