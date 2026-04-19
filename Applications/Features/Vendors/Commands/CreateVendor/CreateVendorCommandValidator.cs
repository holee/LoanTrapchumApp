namespace Applications.Features.Vendors.Commands.CreateVendor;

public sealed class CreateVendorCommandValidator : AbstractValidator<CreateVendorCommand>
{
    public CreateVendorCommandValidator()
    {
        RuleFor(x => x.Dto.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .Matches(@"^[\p{L}\s]+$").WithMessage("First name contains invalid characters.");

        RuleFor(x => x.Dto.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .Matches(@"^[\p{L}\s]+$").WithMessage("Last name contains invalid characters.");

        RuleFor(x => x.Dto.Phone)
            .MaximumLength(20).WithMessage("Phone number must not exceed 20 characters.");

        RuleFor(x => x.Dto.EmailAddress)
            .EmailAddress().When(x => !string.IsNullOrEmpty(x.Dto.EmailAddress))
            .WithMessage("Invalid email address format.");
    }
}
