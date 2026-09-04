using Applications.Features.Periods.Commands.CreatePeriod;
using Applications.Features.Periods.Commands.DeletePeriod;
using Applications.Features.Periods.Commands.UpdatePeriod;
using Applications.Features.Periods.Dto;
using Applications.Features.Periods.Queries.GetPeriods;
using Applications.Responses.Paginations;
using Applications.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.Lists;

public class PeriodsController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetPeriods(
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "Period",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetPeriodsQuery(param));
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePeriod([FromBody] PeriodDtoCreate request)
    {
        var result = await mediator.Send(new CreatePeriodCommand(request));
        return HandleResult(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdatePeriod(int id, [FromBody] PeriodDtoUpdate request)
    {
        request.Id = id;
        var result = await mediator.Send(new UpdatePeriodCommand(request));
        return HandleResult(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePeriod(int id)
    {
        var result = await mediator.Send(new DeletePeriodCommand(id));
        return HandleResult(result);
    }
}
