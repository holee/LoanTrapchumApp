namespace Application.Features.Settings.Queries.UserLoanSizes.GetLoanSizeByUser;

public sealed class GetLoanSizeByUserQueryValidator : AbstractValidator<GetLoanSizeByUserQuery>
{
    public GetLoanSizeByUserQueryValidator()
    {
        RuleFor(x => x.UserId).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("UserId is required.")
            .NotNull().WithMessage("UserId is required.")
            .MaximumLength(450).WithMessage("UserId must not exceed 450 characters.");
    }
}
