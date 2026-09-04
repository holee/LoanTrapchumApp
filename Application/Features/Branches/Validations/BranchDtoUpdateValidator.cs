using Application.Features.Branches.Dtos;
using FluentValidation;

namespace Application.Features.Branches.Validations
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

