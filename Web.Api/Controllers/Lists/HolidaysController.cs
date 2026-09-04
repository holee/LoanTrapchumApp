using Applications.Features.Holidays.Command.CreateHoliday;
using Applications.Features.Holidays.Command.CreateHolidays;
using Applications.Features.Holidays.Command.DeleteHoliday;
using Applications.Features.Holidays.Command.UpdateHoliday;
using Applications.Features.Holidays.Dtos;
using Applications.Features.Holidays.Queries.GetActiveHolidays;
using Applications.Features.Holidays.Queries.GetHolidayById;
using Applications.Features.Holidays.Queries.GetHolidaysAsPaging;
using Applications.Responses.Paginations;
using Applications.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.Lists;

public class HolidaysController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetHolidays(
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "HolidayDate",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetHolidaysAsPagingQuery(param));
        return Ok(result);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActiveHolidays([FromQuery] int? endYear = 1)
    {
        var result = await mediator.Send(new GetActiveHolidaysQuery(endYear));
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetHolidayById(long id)
    {
        var result = await mediator.Send(new GetHolidayByIdQuery(id));
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateHoliday([FromBody] HolidayCreateDto request)
    {
        var result = await mediator.Send(new CreateHolidayCommand(request));
        return HandleResult(result);
    }

    [HttpPost("multiple")]
    public async Task<IActionResult> CreateMultipleHolidays([FromBody] List<HolidayCreateDto> request)
    {
        var result = await mediator.Send(new CreateHolidaysCommand(request));
        return HandleResult(result);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateHoliday(long id, [FromBody] HolidayCreateDto request)
    {
        request.Id = id;
        var result = await mediator.Send(new UpdateHolidayCommand(request));
        return HandleResult(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteHoliday(long id)
    {
        var result = await mediator.Send(new DeleteHolidayCommand(id));
        return HandleResult(result);
    }
}
