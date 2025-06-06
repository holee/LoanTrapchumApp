using Applications.Requests.Lists.Entities;
using FluentValidation;

namespace Applications.Validations.Branches;

public sealed class BranchDtoCreateValidator : AbstractValidator<BranchDtoCreate>
{
    public BranchDtoCreateValidator()
    {
        RuleFor(x=>x.BranchName)
                        .NotEmpty().WithMessage("Branch Name is required.");
        RuleFor(x => x.Description)
                      .NotEmpty().WithMessage("Please fill the branch code.");
    }
}

