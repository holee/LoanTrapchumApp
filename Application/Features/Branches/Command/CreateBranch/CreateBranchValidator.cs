using Application.Features.Branches.Dtos;

namespace Application.Features.Branches.Command.CreateBranch;

public sealed class CreateBranchValidator : AbstractValidator<BranchDtoCreate>
{
    public CreateBranchValidator()
    {

        RuleFor(x => x.BranchName)
            .NotEmpty().WithMessage("Branch name is required.")
            .MaximumLength(100).WithMessage("Branch name must not exceed 100 characters.");

    }
}