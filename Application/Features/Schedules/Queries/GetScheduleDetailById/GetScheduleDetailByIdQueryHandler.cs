using Application.Features.Schedules.Dto;
using ErrorOr;

namespace Application.Features.Schedules.Queries.GetScheduleDetailById;

public sealed class GetScheduleDetailByIdQueryHandler : IRequestHandler<GetScheduleDetailByIdQuery, ErrorOr<ScheduleDetailsDto>>
{
    private readonly ILoanScheduleService _scheduleService;
    public GetScheduleDetailByIdQueryHandler(ILoanScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }
    public async Task<ErrorOr<ScheduleDetailsDto>> Handle(GetScheduleDetailByIdQuery request, CancellationToken cancellationToken)
    {
        var schedule = await _scheduleService.GetScheduleDetailsById(request.BranchId, request.LoanId, request.ScheduleId, cancellationToken);

        if (schedule is null)

            return Error.NotFound("Schedule not found.");

        return schedule;
    }
}