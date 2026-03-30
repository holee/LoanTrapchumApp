using Applications.Features.Schedules.Dto;
using ErrorOr;
using System;
using System.Collections.Generic;
using System.Text;

namespace Applications.Features.Schedules.Queries.GetScheduleDetailById;

public record GetScheduleDetailByIdQuery(int? BranchId, string? LoanId, long? ScheduleId)
    : IRequest<ErrorOr<ScheduleDetailsDto>>
{

}
