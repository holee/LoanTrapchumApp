using Application.Features.Transactions.OpenBranches;
using Application.Responses;
using ErrorOr;
using FluentValidation.Validators;

namespace Application.Features.Branches.Command.OpenBranch;

public record OpenBranchCommand(OpenBranchTransactionDto Transaction) : IRequest<ErrorOr<BranchStatus>>;
