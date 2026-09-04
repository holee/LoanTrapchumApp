using Application.Commons;
using Application.Features.Journals.Command.CreateJournal;
using Application.Features.Journals.Command.UpdateJournal;
using Application.Features.Journals.Dtos;
using Application.Features.Journals.Queries.GetJournalAsPaging;
using Application.Features.Journals.Queries.GetJournalById;
using Application.Features.Journals.Queries.GetJournalEntries;
using Application.Features.Journals.Queries.GetJournalEntryDetailById;
using Application.Features.Journals.Queries.GetJournalEntryForUpdate;
using Application.Features.Journals.Queries.GetJournalTransaction;
using Application.Features.Journals.Queries.GetJournalTypes;
using Application.Features.Journals.Queries.IsJournalNumberExist;
using Application.Responses.Paginations;
using Application.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.Transactions;

public class JournalsController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetJournals(
        [FromQuery] int? branchId = null,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "CreatedAt",
        [FromQuery] bool isDescending = true,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetJournalAsPagingQuery(branchId, param));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetJournalById(string id)
    {
        var result = await mediator.Send(new Application.Features.Journals.Queries.GetJournalById.GetJournalByIdQuery(id));
        return HandleResult(result);
    }

    [HttpGet("{id}/details")]
    public async Task<IActionResult> GetJournalEntryDetailById(string id)
    {
        var result = await mediator.Send(new GetJournalEntryDetailByIdQuery(id));
        return HandleResult(result);
    }

    [HttpGet("{id}/for-update")]
    public async Task<IActionResult> GetJournalEntryForUpdateById(string id, [FromQuery] int? branchId = null)
    {
        var result = await mediator.Send(new GetJournalEntryForUpdateByIdQuery(id, branchId));
        return HandleResult(result);
    }

    [HttpGet("{id}/transaction")]
    public async Task<IActionResult> GetJournalTransaction(string id, [FromQuery] int? branchId = null)
    {
        var result = await mediator.Send(new GetJournalTransactionByIdQuery(id, branchId));
        return HandleResult(result);
    }

    [HttpGet("entries")]
    public async Task<IActionResult> GetJournalEntries(
        [FromQuery] string? journalEntryId = null,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15)
    {
        var param = new QueryParam(pageIndex, pageSize);
        var result = await mediator.Send(new GetJournalEntriesQuery(journalEntryId, param));
        return Ok(result);
    }

    [HttpGet("types")]
    public async Task<IActionResult> GetJournalTypes(
        [FromQuery] int? journalTypeId = null,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15)
    {
        var param = new QueryParam(pageIndex, pageSize);
        var result = await mediator.Send(new GetJournalTypesQuery(journalTypeId, param));
        return Ok(result);
    }

    [HttpGet("check-number")]
    public async Task<IActionResult> IsJournalNumberExist(
        [FromQuery] string? journalNumber = null,
        [FromQuery] int? branchId = null)
    {
        var result = await mediator.Send(new IsJournalNumberExistQuery(journalNumber, branchId));
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateJournal([FromBody] JournalTransactionDtoCreate request)
    {
        var result = await mediator.Send(new CreateJournalCommand(request));
        return HandleResult(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateJournal([FromBody] JournalTransactionDtoUpdate request)
    {
        var result = await mediator.Send(new UpdateJournalCommand(request));
        return HandleResult(result);
    }
}
