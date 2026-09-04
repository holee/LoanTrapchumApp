namespace Application.Features.Branches.Queries.GetBranchState;

public sealed class GetBranchStateQueryValidator : AbstractValidator<GetBranchStateQuery>
{
    public GetBranchStateQueryValidator()
    {
        //RuleFor(x => x.UserId)
        //    .NotNull().WithMessage("UserId is required.")
        //    .Empty().WithMessage("UserId must be not empty.");
    }
}
