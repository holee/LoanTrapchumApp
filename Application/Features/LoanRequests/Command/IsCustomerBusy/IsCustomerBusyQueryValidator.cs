using FluentValidation;

namespace Application.Features.LoanRequests.Command.IsCustomerBusy;

public sealed class IsCustomerBusyQueryValidator : AbstractValidator<IsCustomerBusyQuery>
{
    public IsCustomerBusyQueryValidator()
    {
        RuleFor(x => x.CustomerId).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("CustomerId is required.")
            .NotNull().WithMessage("CustomerId must not be null.");
    }
}
