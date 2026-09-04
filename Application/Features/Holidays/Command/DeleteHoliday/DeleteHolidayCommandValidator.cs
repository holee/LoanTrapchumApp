using FluentValidation;

namespace Application.Features.Holidays.Command.DeleteHoliday;

public sealed class DeleteHolidayCommandValidator : AbstractValidator<DeleteHolidayCommand>
{
    public DeleteHolidayCommandValidator()
    {
        RuleFor(x => x.Id).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Id is required.")
            .GreaterThan(0).WithMessage("Id must be greater than 0.");
    }
}
