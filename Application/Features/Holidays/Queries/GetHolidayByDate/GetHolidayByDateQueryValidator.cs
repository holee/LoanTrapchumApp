namespace Application.Features.Holidays.Queries.GetHolidayByDate;

public sealed class GetHolidayByDateQueryValidator : AbstractValidator<GetHolidayByDateQuery>
{
    public GetHolidayByDateQueryValidator()
    {
        RuleFor(x => x.Dt).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Date is required.")
            .GreaterThan(DateTime.MinValue).WithMessage("Date must be greater than the minimum value.");
    }
}