using Application.Features.Transactions.OpenBranches;

namespace Application.Features.Branches.Command.CloseBranch;

public sealed class CloseBranchTransactionDtoValidator : AbstractValidator<OpenBranchTransactionDto>
{
    public CloseBranchTransactionDtoValidator() 
    {
        RuleFor(x => x.UserRefId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("User reference ID is required.")
            .NotEmpty().WithMessage("User reference ID must be not empty.");

        RuleFor(x => x.BranchRefId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Open branch ID is required.")
            .GreaterThan(0).WithMessage("Open branch ID must be greater than zero.");
        //RuleFor(x => x.OpenBranch).Cascade(CascadeMode.Stop)
        //    .NotNull().WithMessage("Open branch details are required.")
        //    .SetValidator(new OpenBranchDetailsDtoValidator());

        When(x=>x.OpenBranch != null, () =>
        {
            RuleFor(x => x.OpenBranch).SetValidator(new CloseBranchDetailsDtoValidator());
        });
    }
}
