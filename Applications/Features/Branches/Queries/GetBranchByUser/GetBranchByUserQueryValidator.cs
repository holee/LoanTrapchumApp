namespace Applications.Features.Branches.Queries.GetBranchByUser;

public sealed class GetBranchByUserQueryValidator : AbstractValidator<GetBranchByUserQuery>
{
    public GetBranchByUserQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required.");
        //RuleFor(x => x.BranchId).GreaterThan(0).WithMessage("BranchId must be greater than 0.");
    }
}
