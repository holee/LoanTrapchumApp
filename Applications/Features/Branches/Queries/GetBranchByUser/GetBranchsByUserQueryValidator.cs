using Applications.Features.Branches.Queries.GetBranchsByUser;

namespace Applications.Features.Branches.Queries.GetBranchByUser;

public sealed class GetBranchsByUserQueryValidator : AbstractValidator<GetBranchsByUserQuery>
{
    public GetBranchsByUserQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required.");
    }
}
