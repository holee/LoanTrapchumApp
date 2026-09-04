using ErrorOr;

namespace Application.Features.Schedules.Queries.GetScheduleById;

public record GetScheduleByIdQuery(long? ScheduleId, int? BranchId, string? LoanId) : IRequest<ErrorOr<VSchedule>>;
