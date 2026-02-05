using ErrorOr;

namespace Applications.Features.Branches.Command.DeleteBranch;

public record DeleteBranchCommand(int? BranchId) : IRequest<ErrorOr<DeleteResult>>;
