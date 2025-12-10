using FluentValidation;

namespace Applications.Features.Holidays.Command.CreateHolidays;

public sealed class CreateHolidaysCommandValidator : AbstractValidator<CreateHolidaysCommand>
{
    public CreateHolidaysCommandValidator()
    {
        RuleForEach(x => x.Request).SetValidator(new HolidayCreateDtoValidator());
    }
}
