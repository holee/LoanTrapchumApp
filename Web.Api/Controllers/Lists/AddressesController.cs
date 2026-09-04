using Application.Commons;
using Application.Features.Addresses.Command.CreateArea;
using Application.Features.Addresses.Command.CreateCity;
using Application.Features.Addresses.Command.DeleteArea;
using Application.Features.Addresses.Command.UpdateArea;
using Application.Features.Addresses.Command.UpdateCity;
using Application.Features.Addresses.Dto;
using Application.Features.Addresses.Queries.GetAreaById;
using Application.Features.Addresses.Queries.GetAreas;
using Application.Features.Addresses.Queries.GetCities;
using Application.Features.Addresses.Queries.GetCommunes;
using Application.Features.Addresses.Queries.GetCountries;
using Application.Features.Addresses.Queries.GetDistricts;
using Application.Features.Addresses.Queries.GetProvinces;
using Application.Features.Addresses.Queries.GetVillages;
using Application.Responses.Paginations;
using Application.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.Lists;

public class AddressesController(IMediator mediator) : ApiControllerBase
{
    [HttpGet("countries")]
    public async Task<IActionResult> GetCountries(
        [FromQuery] int? countryId = null,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "CountryName",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetCountriesQuery(countryId, param));
        return Ok(result);
    }

    [HttpGet("provinces")]
    public async Task<IActionResult> GetProvinces(
        [FromQuery] int? provinceId = null,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "ProvinceName",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetProvincesQuery(provinceId, param));
        return Ok(result);
    }

    [HttpGet("districts")]
    public async Task<IActionResult> GetDistricts(
        [FromQuery] int? provinceId = null,
        [FromQuery] int? districtId = null,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "DistrictName",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetDistrictsQuery(provinceId, districtId, param));
        return Ok(result);
    }

    [HttpGet("communes")]
    public async Task<IActionResult> GetCommunes(
        [FromQuery] int? districtId = null,
        [FromQuery] int? communeId = null,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "CommuneName",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetCommunesQuery(districtId, communeId, param));
        return Ok(result);
    }

    [HttpGet("villages")]
    public async Task<IActionResult> GetVillages(
        [FromQuery] int? communeId = null,
        [FromQuery] int? villageId = null,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "VillageName",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetVillagesQuery(communeId, villageId, param));
        return Ok(result);
    }

    [HttpGet("cities")]
    public async Task<IActionResult> GetCities(
        [FromQuery] int? cityId = null,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "CityName",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetCitiesQuery(cityId, param));
        return Ok(result);
    }

    [HttpPost("cities")]
    public async Task<IActionResult> CreateCity([FromBody] CityDtoCreate request)
    {
        var result = await mediator.Send(new CreateCityCommand(request));
        return HandleResult(result);
    }

    [HttpPut("cities/{id:int}")]
    public async Task<IActionResult> UpdateCity(int id, [FromBody] CityDtoUpdate request)
    {
        request.Id = id;
        var result = await mediator.Send(new UpdateCityCommand(request));
        return HandleResult(result);
    }

    [HttpGet("areas")]
    public async Task<IActionResult> GetAreas(
        [FromQuery] long? areaId = null,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "AreaName",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetAreasQuery(areaId, param));
        return Ok(result);
    }

    [HttpGet("areas/{id:long}")]
    public async Task<IActionResult> GetAreaById(long id)
    {
        var result = await mediator.Send(new GetAreaByIdQuery(id));
        return HandleResult(result);
    }

    [HttpPost("areas")]
    public async Task<IActionResult> CreateArea([FromBody] AreaDtoCreate request)
    {
        var result = await mediator.Send(new CreateAreaCommand(request));
        return HandleResult(result);
    }

    [HttpPut("areas/{id:long}")]
    public async Task<IActionResult> UpdateArea(long id, [FromBody] AreaDtoUpdate request)
    {
        request.Id = id;
        var result = await mediator.Send(new UpdateAreaCommand(request));
        return HandleResult(result);
    }

    [HttpDelete("areas/{id:long}")]
    public async Task<IActionResult> DeleteArea(long id)
    {
        var result = await mediator.Send(new DeleteAreaCommand(id));
        return HandleResult(result);
    }
}
