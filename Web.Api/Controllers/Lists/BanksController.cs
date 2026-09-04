using Application.Features.Banks.Commands.CreateBank;
using Application.Features.Banks.Commands.DeleteBank;
using Application.Features.Banks.Commands.UpdateBank;
using Application.Features.Banks.Dtos;
using Application.Features.Banks.Queries.GetBankById;
using Application.Features.Banks.Queries.GetBanks;
using Application.Responses.Paginations;
using Application.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.Lists;

public class BanksController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetBanks(
        [FromQuery] int? branchId = null,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "BankName",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetBanksQuery(branchId, param));
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetBankById(int id)
    {
        var result = await mediator.Send(new GetBankByIdQuery(id));
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBank([FromBody] BankDtoCreate request)
    {
        var result = await mediator.Send(new CreateBankCommand(request));
        return HandleResult(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateBank(int id, [FromBody] BankDtoUpdate request)
    {
        var result = await mediator.Send(new UpdateBankCommand(id, request));
        return HandleResult(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBank(int id)
    {
        var result = await mediator.Send(new DeleteBankCommand(id));
        return HandleResult(result);
    }
}
