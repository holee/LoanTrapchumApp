using ErrorOr;

namespace Application.Features.Periods.Commands.DeletePeriod;

public record DeletePeriodCommand(int Id) : IRequest<ErrorOr<bool>>;
