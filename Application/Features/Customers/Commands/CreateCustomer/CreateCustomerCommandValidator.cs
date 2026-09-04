using FluentValidation;

namespace Application.Features.Customers.Commands.CreateCustomer;

public sealed class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.Request)
            .NotNull().WithMessage("Customer request cannot be null.")
            .SetValidator(new CustomerDtoCreateValidator());
    }
}
