namespace Application.Features.Customers.Queries.GetCustomerDetailById;

public sealed class GetCustomerDetailByIdQueryValidator : AbstractValidator<GetCustomerDetailByIdQuery>
{
    public GetCustomerDetailByIdQueryValidator()
    {
        RuleFor(x => x.CustomerId).Cascade(CascadeMode.Stop)
                .NotNull()
                    .WithMessage("Customer Id is required.")
                .NotEmpty()
                    .WithMessage("Customer Id is required.");
    }
}