namespace Application.Features.Branches.Queries.GetUpdateBranch;

public sealed class GetUpdateBranchQueryValidator : AbstractValidator<GetUpdateBranchQuery>
{
    public GetUpdateBranchQueryValidator()
    {
        RuleFor(x => x.BranchId).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Branch Id is required.")
            .GreaterThan(0).WithMessage("Branch Id must be greater than zero.");
    }
}
