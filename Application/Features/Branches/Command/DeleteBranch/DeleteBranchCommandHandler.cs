using Application.Interfaces.Branches;
using ErrorOr;

namespace Application.Features.Branches.Command.DeleteBranch;

public sealed class DeleteBranchCommandHandler : IRequestHandler<DeleteBranchCommand, ErrorOr<DeleteResult>>
{
    private readonly IBranchService _branchRepository;
    public DeleteBranchCommandHandler(IBranchService branchRepository)
    {
        _branchRepository = branchRepository;
    }
    public async Task<ErrorOr<DeleteResult>> Handle(DeleteBranchCommand request, CancellationToken cancellationToken)
    {
        return await _branchRepository.DeleteAsync(request.BranchId, cancellationToken);
    }
}