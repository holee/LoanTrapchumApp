using Applications.Features.Entities;
using FluentValidation;

namespace Applications.Features.Branches.Validations;  

public sealed class BranchDtoCreateValidator : AbstractValidator<BranchDtoCreate>
{
    public BranchDtoCreateValidator()
    {
        RuleFor(x=>x.BranchName)
                        .NotEmpty().WithMessage("Branch Name is required.");
        RuleFor(x => x.BankRefId)
                       .NotNull().WithMessage("Please fill the bank ID.")
                       .GreaterThan(0).WithMessage("Please fill the bank ID.");
    }
}

