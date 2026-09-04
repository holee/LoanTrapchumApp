using Application.Features.AdditionalInformations.Command.CreateNationality;
using Application.Features.AdditionalInformations.Command.CreateOccupation;
using Application.Features.AdditionalInformations.Command.CreateSalutation;
using Application.Features.AdditionalInformations.Command.UpdateOccupation;
using Application.Features.AdditionalInformations.Dto;
using Application.Features.AdditionalInformations.Queries.GetNationalities;
using Application.Features.AdditionalInformations.Queries.GetOccupationDetailsById;
using Application.Features.AdditionalInformations.Queries.GetOccupations;
using Application.Features.AdditionalInformations.Queries.GetSalutaions;
using Application.Responses.Paginations;
using Application.Utils;
using Domains.Shared.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.Lists;

public class AdditionalInformationsController(IMediator mediator) : ApiControllerBase
{
    [HttpGet("occupations")]
    public async Task<IActionResult> GetOccupations(
        [FromQuery] int? id = null,
        [FromQuery] TypeOfEnum type = TypeOfEnum.Position,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "OccupationTitle",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetOccupationsQuery(id, param, type));
        return Ok(result);
    }

    [HttpGet("occupations/{id:int}")]
    public async Task<IActionResult> GetOccupationById(int id)
    {
        var result = await mediator.Send(new GetOccupationDetailsByIdQuery(id));
        return HandleResult(result);
    }

    [HttpPost("occupations")]
    public async Task<IActionResult> CreateOccupation([FromBody] OccupationDtoCreate request)
    {
        var result = await mediator.Send(new CreateOccupationCommand(request));
        return HandleResult(result);
    }

    [HttpPut("occupations/{id:int}")]
    public async Task<IActionResult> UpdateOccupation(int id, [FromBody] OccupationDtoUpate request)
    {
        request.OccupationId = id;
        var result = await mediator.Send(new UpdateOccupationCommand(request));
        return HandleResult(result);
    }

    [HttpGet("nationalities")]
    public async Task<IActionResult> GetNationalities(
        [FromQuery] int? id = null,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "NationalityName",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetNationalitiesQuery(id, param));
        return Ok(result);
    }

    [HttpGet("salutations")]
    public async Task<IActionResult> GetSalutations(
        [FromQuery] int? id = null,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "Title",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetSalutationsQuery(id, param));
        return Ok(result);
    }

    [HttpPost("salutations")]
    public async Task<IActionResult> CreateSalutation([FromBody] SalutationDtoCreate request)
    {
        var result = await mediator.Send(new CreateSalutationCommand(request));
        return HandleResult(result);
    }
}
