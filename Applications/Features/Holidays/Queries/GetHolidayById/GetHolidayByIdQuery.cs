using ErrorOr;

namespace Applications.Features.Holidays.Queries.GetHolidayById;

public record GetHolidayByIdQuery(long? Id) : IRequest<ErrorOr<Holiday>>;
