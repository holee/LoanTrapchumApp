using Application.Features.Schedules.Dto;
using ErrorOr;

namespace Application.Features.LoanPayments.Queries.GetScheduleById;

public sealed record GetScheduleByIdQuery(long? ScheduleId, int? BranchId, string? LoanId) : IRequest<ErrorOr<ScheduleDetailsDto>>;
