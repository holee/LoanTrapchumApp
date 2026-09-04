using FluentValidation;

namespace Application.Features.Collaterals.Queries.GetCollateralByCustomer;

public sealed class GetCollateralByCustomerQueryValidator : AbstractValidator<GetCollateralByCustomerQuery>
{
    public GetCollateralByCustomerQueryValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty().WithMessage("CustomerId is required");    
    }
}