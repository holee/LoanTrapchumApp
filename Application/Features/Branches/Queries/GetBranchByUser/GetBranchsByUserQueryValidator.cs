using Application.Features.Branches.Queries.GetBranchsByUser;

namespace Application.Features.Branches.Queries.GetBranchByUser;

public sealed class GetBranchsByUserQueryValidator : AbstractValidator<GetBranchsByUserQuery>
{
    public GetBranchsByUserQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required.");
    }
}
