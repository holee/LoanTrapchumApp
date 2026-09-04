using Applications.Features.Branches.Command.CreateBranch;
using Applications.Features.Branches.Command.DeleteBranch;
using Applications.Features.Branches.Command.EditBranch;
using Applications.Features.Branches.Dtos;
using Applications.Features.Branches.Queries.GetBranchAsPaging;
using Applications.Features.Branches.Queries.GetBranchById;
using Applications.Features.Branches.Queries.GetBranchDetailsById;
using Applications.Responses.Paginations;
using Applications.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.Lists;

public class BranchesController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetBranches(
        [FromQuery] int? branchId = 0,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "BranchName",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetBranchAsPagingQuery(branchId, param));
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetBranchById(int id)
    {
        var result = await mediator.Send(new GetBranchByIdQuery(id));
        return HandleResult(result);
    }

    [HttpGet("{id:int}/details")]
    public async Task<IActionResult> GetBranchDetails(int id)
    {
        var result = await mediator.Send(new GetBranchDetailsByIdQuery(id));
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBranch([FromBody] BranchDtoCreate request)
    {
        var result = await mediator.Send(new CreateBranchCommand(request));
        return HandleResult(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateBranch(int id, [FromBody] BranchDtoUpdate request)
    {
        request.BranchId = id;
        var result = await mediator.Send(new EditBranchCommand(request));
        return HandleResult(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBranch(int id)
    {
        var result = await mediator.Send(new DeleteBranchCommand(id));
        return HandleResult(result);
    }
}
