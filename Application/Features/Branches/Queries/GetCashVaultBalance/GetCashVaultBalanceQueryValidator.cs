namespace Application.Features.Branches.Queries.GetCashVaultBalance;

public sealed class GetCashVaultBalanceQueryValidator : AbstractValidator<GetCashVaultBalanceQuery>
{
    public GetCashVaultBalanceQueryValidator()
    {
        RuleFor(x => x.BranchId).Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("BranchId is required.")
                .GreaterThan(0).WithMessage("BranchId must be greater than 0.");
    }
}
