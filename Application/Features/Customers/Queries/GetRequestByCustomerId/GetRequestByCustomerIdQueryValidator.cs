namespace Application.Features.Customers.Queries.GetRequestByCustomerId;

public sealed class GetRequestByCustomerIdQueryValidator : AbstractValidator<GetRequestByCustomerIdQuery>
{
    public GetRequestByCustomerIdQueryValidator()
    {
        RuleFor(x => x.CustomerId).Cascade(CascadeMode.Stop)
                .NotNull()
                    .WithMessage("Customer Id is required")
                .NotEmpty()
                    .WithMessage("Customer Id is required");
    }
}