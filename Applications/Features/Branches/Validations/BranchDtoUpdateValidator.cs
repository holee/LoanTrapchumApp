using Applications.Features.Entities;
using FluentValidation;

namespace Applications.Features.Branches.Validations
{
    public sealed class BranchDtoUpdateValidator : AbstractValidator<BranchDtoUpdate>
    {
        public BranchDtoUpdateValidator()
        {
            RuleFor(x => x.BranchName)
                           .NotEmpty().WithMessage("Branch Name is required.");
            RuleFor(x => x.BankRefId)
                           .NotNull().WithMessage("Please fill the bank value.")
                           .GreaterThan(0).WithMessage("Please fill the bank value.");
        }
    }
}

