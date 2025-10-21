using FluentValidation;

namespace Applications.Features.Customers.Commands.GetCustomerActivityQuery;

public sealed class GetCustomerActivityQueryValidator : AbstractValidator<GetCustomerActivityQuery>
{
    public GetCustomerActivityQueryValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty()
                                  .WithMessage("Customer ID is required.");
    }

}