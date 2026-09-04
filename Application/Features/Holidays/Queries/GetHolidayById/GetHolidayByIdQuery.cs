using ErrorOr;

namespace Application.Features.Holidays.Queries.GetHolidayById;

public record GetHolidayByIdQuery(long? Id) : IRequest<ErrorOr<Holiday>>;
