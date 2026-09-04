namespace Application.Features.Customers.Commands.DeleteCustomer;

public sealed class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
{
    public DeleteCustomerCommandValidator()
    {
        RuleFor(x => x.CustomerId).Cascade(CascadeMode.Stop)
                .NotNull()
                .WithMessage("Customer Id is required.")
                    .NotEmpty()
                    .WithMessage("Customer Id is required.");
    }
}