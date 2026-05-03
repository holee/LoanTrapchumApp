using Applications.Features.Holidays.Dtos;
using ErrorOr;

namespace Applications.Features.Holidays.Queries.GetActiveHolidays;

public record GetActiveHolidaysQuery(int? EndYear = 1) : IRequest<Pager<HolidayDto>>;

public sealed class GetActiveHolidaysQueryValidator : AbstractValidator<GetActiveHolidaysQuery>
{
    public GetActiveHolidaysQueryValidator()
    {
        RuleFor(x => x.EndYear).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("EndYear is required.")
            .GreaterThan(0).WithMessage("EndYear must grater than zero.");
    }
}
