using Application.Features.Customers.Commands.CreateCustomer;
using FluentValidation;

namespace Application.Features.Customers.Commands.EditCustomer;

public sealed class EditCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public EditCustomerCommandValidator() 
    {
        RuleFor(x => x.Request)
            .NotNull().WithMessage("Customer request cannot be null.")
            .SetValidator(new CustomerDtoEditValidator());
    }
}
