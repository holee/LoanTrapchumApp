using Application.Commons;
using Application.Features.Ledgers.Commands.ChangeLedgerStatus;
using Application.Features.Ledgers.Commands.CreateLedger;
using Application.Features.Ledgers.Commands.DeleteLedger;
using Application.Features.Ledgers.Dto;
using Application.Features.Ledgers.Queries.GetLedgerByCurrency;
using Application.Features.Ledgers.Queries.GetLedgerById;
using Application.Features.Ledgers.Queries.GetLedgers;
using Application.Features.Ledgers.Queries.GetLedgerTypeById;
using Application.Features.Ledgers.Queries.GetLedgerTypes;
using Application.Responses.Paginations;
using Application.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.Lists;

public class LedgersController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetLedgers(
        [FromQuery] int? currencyId = null,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "LedgerName",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetLedgersPaginatedQuery(currencyId, param));
        return Ok(result);
    }

    [HttpGet("by-currency")]
    public async Task<IActionResult> GetLedgersByCurrency(
        [FromQuery] int? ledgerId = null,
        [FromQuery] int? currencyId = null,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "LedgerName",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetLedgerByCurrencyQuery(ledgerId, currencyId, param));
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetLedgerById(int id)
    {
        var result = await mediator.Send(new GetLedgerByIdQuery(id));
        return HandleResult(result);
    }

    [HttpGet("types")]
    public async Task<IActionResult> GetLedgerTypes(
        [FromQuery] int? ledgerTypeId = null,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "TypeName",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetLedgerTypesQuery(ledgerTypeId, param));
        return Ok(result);
    }

    [HttpGet("types/{id:int}")]
    public async Task<IActionResult> GetLedgerTypeById(int id)
    {
        var result = await mediator.Send(new GetLedgerTypeByIdQuery(id));
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateLedger([FromBody] LedgerCreateDto request)
    {
        var result = await mediator.Send(new CreateLedgerCommand(request));
        return HandleResult(result);
    }

    [HttpPost("change-status")]
    public async Task<IActionResult> ChangeLedgerStatus([FromBody] LedgerChangeStatus request)
    {
        var result = await mediator.Send(new ChangeLedgerStatusCommand(request));
        return HandleResult(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteLedger(int id)
    {
        var result = await mediator.Send(new DeleteLedgerCommand(id));
        return HandleResult(result);
    }
}
