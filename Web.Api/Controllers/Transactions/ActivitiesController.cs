using Applications.Features.Activities.ActitvityQuery.GetActivitiesQuery;
using Applications.Features.Activities.ActitvityQuery.GetActivityByCustomer;
using Applications.Features.Activities.ActitvityQuery.GetActivityById;
using Applications.Features.Activities.Command.CreateActivity;
using Applications.Features.Activities.Command.DeleteActivity;
using Applications.Features.Activities.Command.EditActivity;
using Applications.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.Transactions;

public class ActivitiesController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetActivities([FromQuery] string? customerId = null)
    {
        var result = await mediator.Send(new GetCustomerActivityQuery(customerId));
        return Ok(result);
    }

    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetActivitiesByCustomerId(string customerId)
    {
        var result = await mediator.Send(new GetActivityByCustomerIdQuery(customerId));
        return HandleResult(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetActivityById(int id, [FromQuery] string? customerId = null)
    {
        var result = await mediator.Send(new GetActivityByIdQuery(id, customerId));
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateActivity([FromBody] ActivityDtoCreate request)
    {
        var result = await mediator.Send(new CreateActivityCommand(request));
        return HandleResult(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateActivity(int id, [FromBody] ActivityDtoCreate request)
    {
        request.Id = id;
        var result = await mediator.Send(new EditActivityCommand(request));
        return HandleResult(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteActivity(int id, [FromQuery] string customerId)
    {
        var result = await mediator.Send(new DeleteActivityCommand(id, customerId));
        return HandleResult(result);
    }
}
