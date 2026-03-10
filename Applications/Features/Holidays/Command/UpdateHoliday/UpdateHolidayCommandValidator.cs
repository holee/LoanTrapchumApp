using FluentValidation;

namespace Applications.Features.Holidays.Command.UpdateHoliday;

public sealed class UpdateHolidayCommandValidator : AbstractValidator<UpdateHolidayCommand>
{
    public UpdateHolidayCommandValidator()  
    {
        RuleFor(x => x.Request).SetValidator(new HolidayCreateDtoValidator());
    }
}
