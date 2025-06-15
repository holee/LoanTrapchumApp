using Applications.Requests.Lists.Collaterals;
using FluentValidation;

namespace Applications.Validations;

public sealed class CollateralSubTypeValidator :AbstractValidator<CollateralSubTypeDtoCreate>
{
    public CollateralSubTypeValidator()
    {
        RuleFor(x=>x.SubTypeName)
                            .NotEmpty()
                            .WithMessage("Please enter a subtype name");
        RuleFor(x=>x.LoanSizeAmuntMin)
            .NotNull().WithMessage("Please enter a minimum loan size");
        RuleFor(x=>x.LoanSizeAmuntMax)
            .NotNull().WithMessage("Please enter a maximum loan size");
        RuleFor(x=>x.CollateralTypeRefId)
            .NotNull().WithMessage("Please select a collateral type");
    }
    
}