using Application.Features.Transactions.OpenBranches;
using Application.Responses;
using ErrorOr;

namespace Application.Features.Branches.Command.CloseBranch;

public record CloseBranchCommand(OpenBranchTransactionDto Transaction) : IRequest<ErrorOr<BranchStatus>>;
