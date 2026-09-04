using Applications.Features.Notes.Command.CreateNote;
using Applications.Features.Notes.Command.DeleteNote;
using Applications.Features.Notes.Command.EditNote;
using Applications.Features.Notes.Queries.GetNoteByCustomerId;
using Applications.Features.Notes.Queries.GetNoteById;
using Applications.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.Lists;

public class NotesController(IMediator mediator) : ApiControllerBase
{
    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetNotesByCustomer(string customerId)
    {
        var result = await mediator.Send(new GetNotesByCustomerIdQuery(customerId));
        return HandleResult(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetNoteById(int id, [FromQuery] string? customerId = null)
    {
        var result = await mediator.Send(new GetNoteByIdQuery(id, customerId));
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateNote([FromBody] AddNoteCommand request)
    {
        var result = await mediator.Send(new AddNoteCommand
        {
            NoteId = request.NoteId,
            NoteDate = request.NoteDate,
            CustomerRefId = request.CustomerRefId,
            CreditAgentRefId = request.CreditAgentRefId,
            Description = request.Description
        });
        return HandleResult(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateNote(int id, [FromBody] EditNoteCommand request)
    {
        var result = await mediator.Send(new EditNoteCommand
        {
            NoteId = id,
            NoteDate = request.NoteDate,
            CustomerRefId = request.CustomerRefId,
            CreditAgentRefId = request.CreditAgentRefId,
            Description = request.Description
        });
        return HandleResult(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteNote(int id, [FromQuery] string? customerId = null)
    {
        var result = await mediator.Send(new DeleteNoteCommand(id, customerId));
        return HandleResult(result);
    }
}
