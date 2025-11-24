using FluentValidation;

namespace Applications.Features.Customers.Commands.Customers.CreateCustomer;

public sealed class EditCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public EditCustomerCommandValidator() 
    {
        RuleFor(x => x.Request)
            .NotNull().WithMessage("Customer request cannot be null.")
            .SetValidator(new CustomerDtoEditValidator());
    }
}
