using Applications.Features.Transactions.OpenBranches;
using Applications.Responses;
using ErrorOr;

namespace Applications.Features.Branches.Command.CloseBranch;

public record CloseBranchCommand(OpenBranchTransactionDto Transaction) : IRequest<ErrorOr<BranchStatus>>;
