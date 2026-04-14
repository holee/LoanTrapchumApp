namespace Applications.Features.Customers.Queries.GetCustomerCollateralLine;

public sealed class GetCustomerCollateralLinesQueryValidator : AbstractValidator<GetCustomerCollateralLinesQuery>
{
    public GetCustomerCollateralLinesQueryValidator()
    {
        RuleFor(x => x.CustomerId)
                .NotNull()
                    .WithMessage("Customer Id is required")
                .NotEmpty()
                    .WithMessage("Customer Id is required");
    }
}