namespace Applications.Features.Customers.Commands.DeactivateCustomer;

public sealed class DeactivateCustomerCommandValidator : AbstractValidator<DeactivateCustomerCommand>
{
    public DeactivateCustomerCommandValidator()
    {
        RuleFor(x => x.CustomerId).Cascade(CascadeMode.Stop)
                .NotNull()
                .WithMessage("CustomerId cannot be null.")
            .NotEmpty()
            .WithMessage("CustomerId is required.");
    }
}