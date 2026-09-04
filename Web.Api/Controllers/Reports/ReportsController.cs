using Application.Features.Reports.Queries.PrintLoanScheduleDetails;
using Application.Features.Reports.Queries.PrintLoanScheduleDetailsByGroup;
using Application.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.Reports;

public class ReportsController(IMediator mediator) : ApiControllerBase
{
    [HttpGet("schedule-details")]
    public async Task<IActionResult> GetLoanScheduleDetails(
        [FromQuery] string? loanId = null,
        [FromQuery] int? branchId = null)
    {
        var result = await mediator.Send(new PrintLoanScheduleDetailsQuery(loanId, branchId));
        return HandleResult(result);
    }

    [HttpGet("schedule-details/by-group")]
    public async Task<IActionResult> GetLoanScheduleDetailsByGroup(
        [FromQuery] string? loanId = null,
        [FromQuery] int? branchId = null,
        [FromQuery] int? scheduleGroup = null)
    {
        var result = await mediator.Send(new PrintLoanScheduleDetailsByGroupQuery(loanId, branchId, scheduleGroup));
        return HandleResult(result);
    }
}
