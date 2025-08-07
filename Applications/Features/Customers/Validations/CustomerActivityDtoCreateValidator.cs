using Applications.Features.Customers.Requests;
using FluentValidation;

namespace Applications.Features.Customers.Validations
{
    public sealed class CustomerActivityDtoCreateValidator : AbstractValidator<CustomerActivityDtoCreate>
    {
        public CustomerActivityDtoCreateValidator()
        {
            RuleFor(x => x.CustomerRefId)
                .NotEmpty()
                .WithMessage("Customer reference ID is required")
                .NotNull()
                .WithMessage("Customer reference ID is required");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required")
                .MaximumLength(500)
                .WithMessage("Description must not exceed 500 characters")
                .MinimumLength(2)
                .WithMessage("Description must be at least 2 characters");
        }
    }
}
