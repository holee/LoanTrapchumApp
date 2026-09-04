using FluentValidation;

namespace Application.Features.Holidays.Command.CreateHolidays;

public sealed class CreateHolidaysCommandValidator : AbstractValidator<CreateHolidaysCommand>
{
    public CreateHolidaysCommandValidator()
    {
        RuleForEach(x => x.Request).SetValidator(new HolidayCreateDtoValidator());
    }
}
