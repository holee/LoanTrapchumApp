namespace Application.Features.Holidays.Queries.GetHolidayById;

public sealed class GetHolidayByIdQueryValidator : AbstractValidator<GetHolidayByIdQuery>
{
    public GetHolidayByIdQueryValidator()
    {
        RuleFor(x => x.Id).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Id is required.")
            .GreaterThan(0).WithMessage("Id must grater than zero.");
    }
}