using Application.Enumerations;
using Application.Features.Schedules.GetLoanScheduleBySchedule;
using Application.Features.Schedules.GetLoanWithSchedule;
using Application.Features.Schedules.Queries.GetDueScheduleByLoanId;
using Application.Features.Schedules.Queries.GetScheduleById;
using Application.Features.Schedules.Queries.GetScheduleDetailById;
using Application.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.Transactions;

public class SchedulesController(IMediator mediator) : ApiControllerBase
{
    [HttpGet("loans/{loanId}")]
    public async Task<IActionResult> GetLoanWithSchedule(
        string loanId,
        [FromQuery] int? branchId = null)
    {
        var result = await mediator.Send(new GetLoanWithScheduleQuery(branchId, loanId));
        return HandleResult(result);
    }

    [HttpGet("loans/{loanId}/by-schedule")]
    public async Task<IActionResult> GetLoanScheduleBySchedule(
        string loanId,
        [FromQuery] int? scheduleNum = null,
        [FromQuery] int? branchId = null)
    {
        var result = await mediator.Send(new GetLoanScheduleByScheduleQuery(branchId, loanId, scheduleNum));
        return HandleResult(result);
    }

    [HttpGet("loans/{loanId}/due")]
    public async Task<IActionResult> GetDueSchedules(
        string loanId,
        [FromQuery] int? branchId = null,
        [FromQuery] ScheduleStatus status = ScheduleStatus.Unpaid,
        [FromQuery] DateTime? dueDate = null)
    {
        var result = await mediator.Send(new GetDueScheduleByLoanIdQuery(branchId, loanId, status, dueDate));
        return Ok(result);
    }

    [HttpGet("{scheduleId:long}")]
    public async Task<IActionResult> GetScheduleById(
        long scheduleId,
        [FromQuery] int? branchId = null,
        [FromQuery] string? loanId = null)
    {
        var result = await mediator.Send(new GetScheduleByIdQuery(scheduleId, branchId, loanId));
        return HandleResult(result);
    }

    [HttpGet("{scheduleId:long}/details")]
    public async Task<IActionResult> GetScheduleDetailById(
        long scheduleId,
        [FromQuery] int? branchId = null,
        [FromQuery] string? loanId = null)
    {
        var result = await mediator.Send(new GetScheduleDetailByIdQuery(branchId, loanId, scheduleId));
        return HandleResult(result);
    }
}
