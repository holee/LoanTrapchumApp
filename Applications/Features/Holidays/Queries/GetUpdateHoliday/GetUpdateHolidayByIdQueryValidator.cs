namespace Applications.Features.Holidays.Queries.GetUpdateHoliday;

public sealed class GetUpdateHolidayByIdQueryValidator : AbstractValidator<GetUpdateHolidayByIdQuery>
{
    public GetUpdateHolidayByIdQueryValidator()
    {
        RuleFor(x => x.Id).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Id is required.")
            .GreaterThan(0).WithMessage("Id must grater than zero.");
    }
}