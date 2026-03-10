using FluentValidation;

namespace Applications.Features.Holidays.Command.ActivateOrDeactivate;

public sealed class ActivateOrDeactivateHolidayCommandValidator : AbstractValidator<ActivateOrDeactivateHolidayCommand>
{
    public ActivateOrDeactivateHolidayCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage("Holiday ID is required.")
            .GreaterThan(0).WithMessage("Holiday ID must be greater than zero.");

    }
}
