namespace Application.Features.Branches.Command.ChangeBranchState;

public sealed class ChangeBranchStateCommandValidator : AbstractValidator<ChangeBranchStateCommand>
{
    public ChangeBranchStateCommandValidator()
    {
        RuleFor(x => x.BranchId).Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("BranchId is required.")
                .NotEmpty().WithMessage("BranchId cannot be empty.");
        RuleFor(x => x.State).NotNull().WithMessage("State is required.");
    }
}
