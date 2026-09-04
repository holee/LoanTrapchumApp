using Application.Interfaces.Transactions;
using Application.Responses;
using Domains.Models.Transaction;
using ErrorOr;
using Mapster;

namespace Application.Features.Branches.Command.OpenBranch;

public sealed class OpenBranchCommandHandler(IOpenBranchService obService) : IRequestHandler<OpenBranchCommand, ErrorOr<BranchStatus>>
{
    public async Task<ErrorOr<BranchStatus>> Handle(OpenBranchCommand request, CancellationToken cancellationToken)
    {

        var req = request.Transaction.Adapt<LoanTransaction>();

        return await obService.OpenBranchAsync(req, cancellationToken);


    }
}
