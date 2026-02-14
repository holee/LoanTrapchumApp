using Applications.Features.Transactions.OpenBranches;
using Applications.Responses;
using ErrorOr;
using FluentValidation.Validators;

namespace Applications.Features.Branches.Command.OpenBranch;

public record OpenBranchCommand(OpenBranchTransactionDto Transaction) : IRequest<ErrorOr<BranchStatus>>;
