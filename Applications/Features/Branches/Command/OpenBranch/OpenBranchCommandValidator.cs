namespace Applications.Features.Branches.Command.OpenBranch;

public sealed class OpenBranchCommandValidator : AbstractValidator<OpenBranchCommand>
{
    public OpenBranchCommandValidator()
    {
        RuleFor(x => x.Transaction)
            .NotNull().WithMessage("Transaction data is required.")
            .SetValidator(new OpenBranchTransactionDtoValidator());
    }
}
