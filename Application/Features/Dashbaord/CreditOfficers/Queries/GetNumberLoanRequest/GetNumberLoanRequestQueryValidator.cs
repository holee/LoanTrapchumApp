namespace Application.Features.Dashbaord.CreditOfficers.Queries.GetNumberLoanRequest;

public sealed class GetNumberLoanRequestQueryValidator : AbstractValidator<GetNumberLoanRequestQuery>
{
    public GetNumberLoanRequestQueryValidator()
    {
        RuleFor(x => x.BranchId).Cascade(CascadeMode.Stop)
                        .NotEmpty().WithMessage("BranchId is required.")
                        .GreaterThan(0)
                        .WithMessage("BranchId must be greater than 0.");
        RuleFor(x => x.UserId).Cascade(CascadeMode.Stop)
                        .NotEmpty().WithMessage("UserId is required.")
                        .NotNull().WithMessage("UserId cannot be null.");
    }
}