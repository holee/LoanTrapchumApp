using Applications.Features.Branches.Dtos;

namespace Applications.Features.Branches.Command.EditBranch;

public sealed class EditBranchValidator : AbstractValidator<BranchDtoUpdate>
{
    public EditBranchValidator()
    {

        RuleFor(x => x.BranchName)
            .NotEmpty().WithMessage("Branch name is required.")
            .MaximumLength(100).WithMessage("Branch name must not exceed 100 characters.");

    }
}