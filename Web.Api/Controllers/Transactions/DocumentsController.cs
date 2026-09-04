using Application.Features.Documents.Command.AddDocument;
using Application.Features.Documents.Command.AddDocuments;
using Application.Features.Documents.Command.DeleteDocument;
using Application.Features.Documents.Dto;
using Application.Features.Documents.Queries.GetDocumentById;
using Application.Features.Documents.Queries.GetDocuments;
using Application.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.Transactions;

public class DocumentsController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetDocuments()
    {
        var result = await mediator.Send(new GetDocumentsQuery());
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetDocumentById(Guid id)
    {
        var result = await mediator.Send(new GetDocumentByIdQuery(id));
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddDocument([FromBody] DocumentDtoCreate request)
    {
        var result = await mediator.Send(new AddDocumentCommand(request));
        return HandleResult(result);
    }

    [HttpPost("batch")]
    public async Task<IActionResult> AddDocuments([FromBody] IEnumerable<DocumentDtoCreate> request)
    {
        var result = await mediator.Send(new AddDocumentsCommand(request));
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteDocument(Guid id)
    {
        var result = await mediator.Send(new DeleteDocumentCommand(id));
        return Ok(result);
    }
}
