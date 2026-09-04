using Applications.Features.Collaterals.Commands.AddOrUpdateCollaterals;
using Applications.Features.Collaterals.Commands.DeleteCollateralLines;
using Applications.Features.Collaterals.Dtos;
using Applications.Features.Collaterals.Queries.GetCollateralAsPaging;
using Applications.Features.Collaterals.Queries.GetCollateralByCustomer;
using Applications.Features.Collaterals.Queries.GetCollateralSubTypesById;
using Applications.Features.Collaterals.Queries.GetCollateralTypes;
using Applications.Responses.Paginations;
using Applications.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.Lists;

public class CollateralsController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCollaterals(
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "Id",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetCollateralAsPagingQuery(param));
        return Ok(result);
    }

    [HttpGet("types")]
    public async Task<IActionResult> GetCollateralTypes([FromQuery] int? typeId = null, [FromQuery] string? typeName = null)
    {
        var result = await mediator.Send(new GetCollateralTypeQuery(typeId, typeName));
        return Ok(result);
    }

    [HttpGet("sub-types")]
    public async Task<IActionResult> GetCollateralSubTypes([FromQuery] int? collateralTypeId = null, [FromQuery] string? searchTerm = null)
    {
        var result = await mediator.Send(new GetCollateralSubTypesByIdQuery(collateralTypeId, searchTerm));
        return Ok(result);
    }

    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetCollateralByCustomer(string customerId, [FromQuery] bool include = true)
    {
        var result = await mediator.Send(new GetCollateralByCustomerQuery(customerId, include));
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddOrUpdateCollateral([FromBody] CollateralDtoCreate request, [FromQuery] bool detaching = false)
    {
        var result = await mediator.Send(new AddOrUpdateCollateralCommand(request, detaching));
        return HandleResult(result);
    }

    [HttpDelete("lines/{lineId:long}")]
    public async Task<IActionResult> DeleteCollateralLine(long lineId, [FromQuery] string collateralId)
    {
        var result = await mediator.Send(new DeleteCollateralLineCommand(lineId, collateralId));
        return HandleResult(result);
    }
}
