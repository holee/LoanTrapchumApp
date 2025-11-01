using FluentValidation;

namespace Applications.Features.Collaterals.Queries.GetCollateralByCustomer;

public sealed class GetCollateralByCustomerQueryValidator : AbstractValidator<GetCollateralByCustomerQuery>
{
    public GetCollateralByCustomerQueryValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty().WithMessage("CustomerId is required");    
    }
}