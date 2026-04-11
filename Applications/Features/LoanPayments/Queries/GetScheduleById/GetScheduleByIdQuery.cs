using Applications.Features.Schedules.Dto;
using ErrorOr;

namespace Applications.Features.LoanPayments.Queries.GetScheduleById;

public sealed record GetScheduleByIdQuery(long? ScheduleId, int? BranchId, string? LoanId) : IRequest<ErrorOr<ScheduleDetailsDto>>;
