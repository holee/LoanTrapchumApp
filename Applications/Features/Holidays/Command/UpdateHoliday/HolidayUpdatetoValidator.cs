using Applications.Features.Holidays.Dtos;
using FluentValidation;

namespace Applications.Features.Holidays.Command.CreateHolidays;

public sealed class HolidayUpdatetoValidator : AbstractValidator<HolidayCreateDto>
{
    public HolidayUpdatetoValidator()
    {

        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Holiday ID is required.");

        RuleFor(x => x.HolidayDt)
            .NotEmpty().WithMessage("Holiday date is required.")
            .LessThanOrEqualTo(DateTime.MaxValue).WithMessage("Holiday date must be a valid date.");
        RuleFor(x => x.HolidayName)
            .NotEmpty().WithMessage("Holiday name is required.")
            .MaximumLength(100).WithMessage("Holiday name must not exceed 100 characters.");
        RuleFor(x => x.HolidayNum)
            .NotEmpty().WithMessage("Holiday number is required.")
            .GreaterThan(0).WithMessage("Holiday number must be greater than 0.");
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
    }
}