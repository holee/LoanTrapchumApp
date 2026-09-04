using Application.Features.Currencies.Command;
using Application.Features.Currencies.Command.ActivateOrDeactivateNote;
using Application.Features.Currencies.Command.AddCurrencyNote;
using Application.Features.Currencies.Command.AddNewCurrency;
using Application.Features.Currencies.Command.EditCurrencyNote;
using Application.Features.Currencies.Command.EditMoneyExchange;
using Application.Features.Currencies.Command.SetSpotRate;
using Application.Features.Currencies.Command.SetSpotRates;
using Application.Features.Currencies.Dto;
using Application.Features.Currencies.Queries.GetCashVault;
using Application.Features.Currencies.Queries.GetCurrencies;
using Application.Features.Currencies.Queries.GetCurrenciesAsPaging;
using Application.Features.Currencies.Queries.GetCurrenciesForSetSpotRates;
using Application.Features.Currencies.Queries.GetCurrencyCounter;
using Application.Features.Currencies.Queries.GetCurrencyDetails;
using Application.Features.Currencies.Queries.GetCurrencyNotes;
using Application.Features.Currencies.Queries.GetEditMoneyExchange;
using Application.Features.Currencies.Queries.GetSpotRates;
using Application.Features.Transactions.ForiegnExchanges;
using Application.Responses.Paginations;
using Application.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.Lists;

public class CurrenciesController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCurrencies(
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "CurrencyCode",
        [FromQuery] bool isDescending = false,
        [FromQuery] bool includeInactive = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, includeInactive, filter);
        var result = await mediator.Send(new GetCurrencyAsPaging(param));
        return Ok(result);
    }

    [HttpGet("lookup")]
    public async Task<IActionResult> GetCurrencyLookup(
        [FromQuery] int? currencyId = 0,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15)
    {
        var param = new QueryParam(pageIndex, pageSize);
        var result = await mediator.Send(new GetCurrenciesQuery(currencyId, param));
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCurrencyById(int id)
    {
        var result = await mediator.Send(new GetCurrencyByIdQuery(id));
        return HandleResult(result);
    }

    [HttpGet("{id:int}/details")]
    public async Task<IActionResult> GetCurrencyDetailsById(int id)
    {
        var result = await mediator.Send(new GetCurrencyDetailsByIdQuery(id));
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCurrency([FromBody] CurrencyDtoCreate request)
    {
        var result = await mediator.Send(new AddNewCurrencyCommand(request));
        return HandleResult(result);
    }

    [HttpGet("{id:int}/spot-rates")]
    public async Task<IActionResult> GetSpotRates(
        int id,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15)
    {
        var param = new QueryParam(pageIndex, pageSize);
        var result = await mediator.Send(new GetSpotRatesQuery(id, param));
        return Ok(result);
    }

    [HttpPost("{id:int}/spot-rate")]
    public async Task<IActionResult> SetSpotRate(int id, [FromBody] CurrencySpotRateDtoCreate request)
    {
        var result = await mediator.Send(new SetSpotRateCommand(id, request));
        return HandleResult(result);
    }

    [HttpPost("spot-rates")]
    public async Task<IActionResult> SetSpotRates([FromBody] List<CurrencySpotRateDtoCreate> request)
    {
        var result = await mediator.Send(new SetSpotRatesCommand(request));
        return HandleResult(result);
    }

    [HttpGet("notes")]
    public async Task<IActionResult> GetCurrencyNotes(
        [FromQuery] int? currencyId = null,
        [FromQuery] int? branchId = null)
    {
        var result = await mediator.Send(new GetCurrencyCounterQuery(currencyId, branchId));
        return Ok(result);
    }

    [HttpPost("notes")]
    public async Task<IActionResult> CreateCurrencyNote([FromBody] CurrencyNoteDtoCreate request)
    {
        var result = await mediator.Send(new AddCurrencyNoteCommand(request));
        return HandleResult(result);
    }

    [HttpPut("notes")]
    public async Task<IActionResult> UpdateCurrencyNote([FromBody] CurrencyNoteDtoUpdate request)
    {
        var result = await mediator.Send(new EditCurrencyNoteCommand(request));
        return HandleResult(result);
    }

    [HttpPatch("notes/{id:int}/toggle")]
    public async Task<IActionResult> ToggleNote(int id, [FromBody] NoteToggleRequest request)
    {
        var result = await mediator.Send(new ToggleNoteCommand(id, new NoteActivateDto(request.IsActive)));
        return HandleResult(result);
    }

    [HttpGet("money-exchanges")]
    public async Task<IActionResult> GetMoneyExchanges(
        [FromQuery] int? branchId = null,
        [FromQuery] DateTimeOffset? transactionDate = null,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "CreatedAt",
        [FromQuery] bool isDescending = true,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetMoneyExchangeAsPagingQuery(branchId, transactionDate, param));
        return Ok(result);
    }

    [HttpGet("money-exchanges/{transactionId}")]
    public async Task<IActionResult> GetMoneyExchangeById(
        string transactionId,
        [FromQuery] int? branchId = null,
        [FromQuery] DateTimeOffset? transactionDate = null)
    {
        var result = await mediator.Send(new GetEditMoneyExchangeQuery(branchId, transactionId, transactionDate ?? DateTimeOffset.UtcNow));
        return HandleResult(result);
    }

    [HttpPost("money-exchanges")]
    public async Task<IActionResult> CreateMoneyExchange([FromBody] ForeignExchangeDtoCreate request)
    {
        var result = await mediator.Send(new CreateMoneyExchangeCommand(request));
        return HandleResult(result);
    }

    [HttpPut("money-exchanges/{id:long}")]
    public async Task<IActionResult> EditMoneyExchange(long id, [FromBody] ForeignExchangeDtoUpdate request)
    {
        request.Id = id;
        var result = await mediator.Send(new EditMoneyExchangeCommand(request));
        return HandleResult(result);
    }
}
