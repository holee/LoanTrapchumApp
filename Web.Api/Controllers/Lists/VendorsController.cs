using Applications.Features.OtherNames.Dto;
using Applications.Features.Vendors.Commands.CreateVendor;
using Applications.Features.Vendors.Commands.DeleteVendor;
using Applications.Features.Vendors.Commands.UpdateVendor;
using Applications.Features.Vendors.Queries.GetVendorById;
using Applications.Features.Vendors.Queries.GetVendorsAsPaging;
using Applications.Responses.Paginations;
using Applications.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.Lists;

public class VendorsController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetVendors(
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "VendorName",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetVendorsAsPagingQuery(param));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetVendorById(string id)
    {
        var result = await mediator.Send(new GetVendorByIdQuery(id));
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateVendor([FromBody] EntityVendorCreate request)
    {
        var result = await mediator.Send(new CreateVendorCommand(request));
        return HandleResult(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateVendor(string id, [FromBody] EntityVendorCreate request)
    {
        request.VendorId = id;
        var result = await mediator.Send(new UpdateVendorCommand(request));
        return HandleResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVendor(string id)
    {
        var result = await mediator.Send(new DeleteVendorCommand(id));
        return HandleResult(result);
    }
}
