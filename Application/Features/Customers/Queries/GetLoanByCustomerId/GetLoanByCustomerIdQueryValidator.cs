namespace Application.Features.Customers.Queries.GetLoanByCustomerId;

public sealed class GetLoanByCustomerIdQueryValidator : AbstractValidator<GetLoanByCustomerIdQuery>
{
    public GetLoanByCustomerIdQueryValidator()
    {
        RuleFor(x => x.CustomerId).Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage("Customer Id is required")
                .NotNull()
                    .WithMessage("Customer Id cannot be null");
    }
}