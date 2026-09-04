namespace Application.Features.LoanRequests.Queryies.IsCoPawnExist;

public sealed class IsCoPawnExistQueryValidator : AbstractValidator<IsCoPawnExistQuery>
{
    public IsCoPawnExistQueryValidator()
    {
        RuleFor(x => x.CustomerId).Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithMessage("CustomerId is required.")
            .NotNull()
                .WithMessage("CustomerId cannot be null.");
        RuleFor(x => x.BranchId).Cascade(CascadeMode.Stop)
            .NotEmpty()
                .WithMessage("BranchId is required.")
            .NotNull()
                .WithMessage("BranchId cannot be null.");
    }
}
