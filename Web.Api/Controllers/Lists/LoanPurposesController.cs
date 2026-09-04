using Applications.Features.LoanPurposes;
using Applications.Features.LoanPurposes.Commands.CreateLoanPurpose;
using Applications.Features.LoanPurposes.Commands.DeleteLoanPurpose;
using Applications.Features.LoanPurposes.Commands.UpdateLoanPurpose;
using Applications.Features.LoanPurposes.Queries.GetLoanPurposeById;
using Applications.Features.LoanPurposes.Queries.GetLoanPurposes;
using Applications.Responses.Paginations;
using Applications.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.Lists;

public class LoanPurposesController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetLoanPurposes(
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "Title",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetLoanPurposesQuery(param));
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetLoanPurposeById(int id)
    {
        var result = await mediator.Send(new GetLoanPurposeByIdQuery(id));
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateLoanPurpose([FromBody] LoanPurposeDtoCreate request)
    {
        var result = await mediator.Send(new CreateLoanPurposeCommand(request));
        return HandleResult(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateLoanPurpose(int id, [FromBody] LoanPurposeDtoUpdate request)
    {
        request.Id = id;
        var result = await mediator.Send(new UpdateLoanPurposeCommand(request));
        return HandleResult(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteLoanPurpose(int id)
    {
        var result = await mediator.Send(new DeleteLoanPurposeCommand(id));
        return HandleResult(result);
    }
}
