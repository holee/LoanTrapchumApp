namespace Applications.Features.Branches.Command.CloseBranch;

public sealed class CloseBranchCommandValidator : AbstractValidator<CloseBranchCommand>
{
    public CloseBranchCommandValidator()
    {
        RuleFor(x => x.Transaction)
            .NotNull().WithMessage("Transaction data is required.")
            .SetValidator(new CloseBranchTransactionDtoValidator());
    }
}
