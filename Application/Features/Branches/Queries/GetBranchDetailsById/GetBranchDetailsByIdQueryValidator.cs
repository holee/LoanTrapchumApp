namespace Application.Features.Branches.Queries.GetBranchDetailsById;

public sealed class GetBranchDetailsByIdQueryValidator : AbstractValidator<GetBranchDetailsByIdQuery>
{
    public GetBranchDetailsByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Branch Id must be greater than zero.");
    }
}
