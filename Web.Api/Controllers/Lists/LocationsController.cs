using Applications.Features.Addresses.Interfaces;
using Applications.Features.Locations.Command.CreateLocation;
using Applications.Features.Locations.Command.DeleteLocation;
using Applications.Features.Locations.Command.UpdateLocation;
using Applications.Features.Locations.Dtos;
using Applications.Features.Locations.Queries.GetAllLocations;
using Applications.Features.Locations.Queries.GetLocationById;
using Applications.Features.Locations.Queries.GetLocationsAsPaging;
using Applications.Responses.Paginations;
using Applications.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.Lists;

public class LocationsController(IMediator mediator, IAddressServiceImpl addressService) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetLocations(
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "Id",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetLocationsAsPagingQuery(param));
        return Ok(result);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllLocations()
    {
        var result = await mediator.Send(new GetAllLocationsQuery());
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetLocationById(long id)
    {
        var result = await mediator.Send(new GetLocationByIdQuery(id));
        return HandleResult(result);
    }

    [HttpGet("provinces")]
    public async Task<IActionResult> GetProvinces([FromQuery] string? filter = null)
    {
        var result = await addressService.GetsProvinces(filter);
        return Ok(result);
    }

    [HttpGet("districts/{provinceId:int}")]
    public async Task<IActionResult> GetDistricts(int provinceId)
    {
        var result = await addressService.GetDistrictsByProvinceId(provinceId);
        return Ok(result);
    }

    [HttpGet("communes/{districtId:int}")]
    public async Task<IActionResult> GetCommunes(int districtId)
    {
        var result = await addressService.GetCommunesByDistrictId(districtId);
        return Ok(result);
    }

    [HttpGet("villages/{communeId:int}")]
    public async Task<IActionResult> GetVillages(int communeId)
    {
        var result = await addressService.GetVillagesByCommuneId(communeId);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateLocation([FromBody] LocationCreateDto request)
    {
        var result = await mediator.Send(new CreateLocationCommand(request));
        return HandleResult(result);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateLocation(long id, [FromBody] LocationCreateDto request)
    {
        request.Id = id;
        var result = await mediator.Send(new UpdateLocationCommand(request));
        return HandleResult(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteLocation(long id)
    {
        var result = await mediator.Send(new DeleteLocationCommand(id));
        return HandleResult(result);
    }
}
