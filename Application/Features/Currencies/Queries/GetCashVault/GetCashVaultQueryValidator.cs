namespace Application.Features.Currencies.Queries.GetCashVault;

public sealed class GetCashVaultQueryValidator : AbstractValidator<GetCashVaultQuery>
{
    public GetCashVaultQueryValidator()
    {
        RuleFor(x => x.BranchId).Cascade(CascadeMode.Stop)
                    .NotNull().WithMessage("BranchId is required.")
                    .GreaterThan(0).WithMessage("BranchId must be greater than 0.");
    }
}
