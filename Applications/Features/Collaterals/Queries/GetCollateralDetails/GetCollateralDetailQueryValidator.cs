using Applications.Features.Collaterals.Queries.GetCollateralDetails;
using FluentValidation;

namespace Applications.Features.Collaterals.Queries.GetCollateralByCustomers;

public sealed class GetCollateralDetailQueryValidator : AbstractValidator<GetCollateralDetailQuery>
{
    public GetCollateralDetailQueryValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Customer ID is required.");
    }
};
