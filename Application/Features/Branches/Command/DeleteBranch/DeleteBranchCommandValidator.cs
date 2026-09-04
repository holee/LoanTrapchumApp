namespace Application.Features.Branches.Command.DeleteBranch;

public sealed class DeleteBranchCommandValidator : AbstractValidator<DeleteBranchCommand>
{
    public DeleteBranchCommandValidator()
    {
        RuleFor(x => x.BranchId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("BranchId is required.")
            .GreaterThan(0).WithMessage("BranchId must be greater than zero.");
    }
}
