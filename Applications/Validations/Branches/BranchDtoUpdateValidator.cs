using Applications.Requests.Lists.Entities;
using FluentValidation;
namespace Applications.Validations.Branches
{
    public sealed class BranchDtoUpdateValidator : AbstractValidator<BranchDtoUpdate>
    {
        public BranchDtoUpdateValidator()
        {
            RuleFor(x => x.BranchName)
                           .NotEmpty().WithMessage("Branch Name is required.");
            RuleFor(x => x.Description)
                          .NotEmpty().WithMessage("Please fill the branch code.");
        }
    }
}

