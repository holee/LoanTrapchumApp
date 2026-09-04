using Applications.Features.Guarantor.Command.CreateGuarantor;
using Applications.Features.Guarantor.Command.DeleteGuarantor;
using Applications.Features.Guarantor.Command.UpdateGuarantor;
using Applications.Features.Guarantor.Dtos;
using Applications.Features.Guarantor.Queries.GetGuarantorById;
using Applications.Features.Guarantor.Queries.GetGuarantorsAsPaging;
using Applications.Responses.Paginations;
using Applications.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.Lists;

public class GuarantorsController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetGuarantors(
        [FromQuery] string? id = null,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "GuarantorName",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetGuarantorsAsPagingQuery(id, param));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGuarantorById(string id, [FromQuery] bool allGuarantors = false, [FromQuery] bool includeRequest = true)
    {
        var result = await mediator.Send(new GetGuarantorByIdQuery(id, allGuarantors, includeRequest));
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateGuarantor([FromBody] EntityGuarantorCreate request)
    {
        var result = await mediator.Send(new CreateGuarantorCommand(request));
        return HandleResult(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGuarantor(string id, [FromBody] EntityGuarantorCreate request)
    {
        request.Id = id;
        var result = await mediator.Send(new UpdateGuarantorCommand(request));
        return HandleResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGuarantor(string id)
    {
        var result = await mediator.Send(new DeleteGuarantorCommand(id));
        return HandleResult(result);
    }
}
