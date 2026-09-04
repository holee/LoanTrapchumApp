using Applications.Features.OtherNames.Commands.ConvertToCustomer;
using Applications.Features.OtherNames.Commands.ConvertToEmployee;
using Applications.Features.OtherNames.Commands.ConvertToVendor;
using Applications.Features.OtherNames.Commands.CreateOtherName;
using Applications.Features.OtherNames.Commands.DeleteOtherName;
using Applications.Features.OtherNames.Commands.UpdateOtherName;
using Applications.Features.OtherNames.Dto;
using Applications.Features.OtherNames.Queries.GetOtherNameById;
using Applications.Features.OtherNames.Queries.GetOtherNamesAsPaging;
using Applications.Responses.Paginations;
using Applications.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.Lists;

public class OtherNamesController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetOtherNames(
        [FromQuery] string? type = null,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "Name",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetOtherNamesAsPagingQuery(param, type));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOtherNameById(string id)
    {
        var result = await mediator.Send(new GetOtherNameByIdQuery(id));
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOtherName([FromBody] EntityNamesCreate request)
    {
        var result = await mediator.Send(new CreateOtherNameCommand(request));
        return HandleResult(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOtherName(string id, [FromBody] EntityNamesCreate request)
    {
        request.Id = id;
        var result = await mediator.Send(new UpdateOtherNameCommand(request));
        return HandleResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOtherName(string id)
    {
        var result = await mediator.Send(new DeleteOtherNameCommand(id));
        return HandleResult(result);
    }

    [HttpPost("{id}/convert-to-customer")]
    public async Task<IActionResult> ConvertToCustomer(string id, [FromBody] ConvertCreateCustomer request)
    {
        var result = await mediator.Send(new ConvertToCustomerCommand(id, request));
        return HandleResult(result);
    }

    [HttpPost("{id}/convert-to-employee")]
    public async Task<IActionResult> ConvertToEmployee(string id, [FromBody] ConvertCreateEmployee request)
    {
        var result = await mediator.Send(new ConvertToEmployeeCommand(id, request));
        return HandleResult(result);
    }

    [HttpPost("{id}/convert-to-vendor")]
    public async Task<IActionResult> ConvertToVendor(string id, [FromBody] ConvertCreateVendor request)
    {
        var result = await mediator.Send(new ConvertToVendorCommand(id, request));
        return HandleResult(result);
    }
}
