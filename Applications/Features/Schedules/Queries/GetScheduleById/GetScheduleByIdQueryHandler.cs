using ErrorOr;

namespace Applications.Features.Schedules.Queries.GetScheduleById;

public sealed class GetScheduleByIdQueryHandler : IRequestHandler<GetScheduleByIdQuery, ErrorOr<VSchedule>>
{
    private readonly ILoanScheduleService _service;

    public GetScheduleByIdQueryHandler(ILoanScheduleService service)
    {
        _service = service;
    }

    public async Task<ErrorOr<VSchedule>> Handle(GetScheduleByIdQuery request, CancellationToken cancellationToken)
    {
        var schedule = await _service.GetScheduleByIdAsync(request.ScheduleId, request.BranchId, request.LoanId);

        if (schedule is null)
        {
            return Error.NotFound("Schedule.NotFound", "The specified schedule was not found.");
        }
        return schedule;
    }
}