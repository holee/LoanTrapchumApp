using ErrorOr;

namespace Applications.Features.Periods.Commands.DeletePeriod;

public record DeletePeriodCommand(int Id) : IRequest<ErrorOr<bool>>;
