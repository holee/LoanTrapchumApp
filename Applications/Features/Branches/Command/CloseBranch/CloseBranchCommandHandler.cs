using Applications.Interfaces.Transactions;
using Applications.Responses;
using Domains.Models.Transaction;
using ErrorOr;
using Mapster;

namespace Applications.Features.Branches.Command.CloseBranch;

public sealed class CloseBranchCommandHandler(IOpenBranchService obService) : IRequestHandler<CloseBranchCommand, ErrorOr<BranchStatus>>
{
    public async Task<ErrorOr<BranchStatus>> Handle(CloseBranchCommand request, CancellationToken cancellationToken)
    {

        var req = request.Transaction.Adapt<LoanTransaction>();

        return await obService.CloseBranchAsync(req, cancellationToken);


    }
}
