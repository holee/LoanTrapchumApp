namespace Application.Features.Branches.Queries.GetBranchById;

public sealed class GetBranchByIdQueryValidator : AbstractValidator<GetBranchByIdQuery>
{
    public GetBranchByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Branch Id must be greater than zero.");
    }
}
