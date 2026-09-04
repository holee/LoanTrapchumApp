using FluentValidation;

namespace Application.Features.Holidays.Command.CreateHoliday;

public sealed class CreateHolidayCommandValidator : AbstractValidator<CreateHolidayCommand>
{
    public CreateHolidayCommandValidator()
    {
        RuleFor(x => x.Request.HolidayName)
            .NotEmpty().WithMessage("Holiday name is required.")
            .MaximumLength(100).WithMessage("Holiday name must not exceed 100 characters.");
        RuleFor(x => x.Request.HolidayDt)
            .NotEmpty().WithMessage("Holiday date is required.")
            .Must(date => date > DateTime.MinValue).WithMessage("Holiday date is invalid.");
        RuleFor(x => x.Request.HolidayNum)
            .GreaterThan(0).WithMessage("Holiday number must be greater than zero.");
        RuleFor(x => x.Request.Status)
            .NotNull().WithMessage("Status is required.");
        RuleFor(x => x.Request.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
    }
}
