namespace Application.Features.Branches.Queries.GetBranchForVault;

public class GetBranchForVaultQueryValidator : AbstractValidator<GetBranchForVaultQuery>
{
    public GetBranchForVaultQueryValidator()
    {
        RuleFor(x => x.BranchId)
            .NotNull().WithMessage("BranchId is required.")
            .GreaterThan(0).WithMessage("BranchId must be greater than 0.");
    }
}