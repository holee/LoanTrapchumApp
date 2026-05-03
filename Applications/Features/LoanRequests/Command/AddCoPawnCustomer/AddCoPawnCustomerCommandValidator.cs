namespace Applications.Features.LoanRequests.Command.AddCoPawnCustomer;

public sealed class AddCoPawnCustomerCommandValidator : AbstractValidator<AddCoPawnCustomerCommand>
{
    public AddCoPawnCustomerCommandValidator()
    {
        RuleFor(x => x.Request).NotNull().WithMessage("Request cannot be null.");
        RuleFor(x => x.Request.BranchId)
                .NotNull()
                    .WithMessage("Branch ID is required.")
                .GreaterThan(0)
                    .WithMessage("Branch ID must be greater than 0.");
        //RuleFor(x => x.Request.LastName).NotEmpty().WithMessage("Last name is required.");
        //RuleFor(x => x.Request.Email).EmailAddress().WithMessage("Invalid email address.");
        //RuleFor(x => x.Request.PhoneNumber).NotEmpty().WithMessage("Phone number is required.");
        // Add more validation rules as needed
    }
}
