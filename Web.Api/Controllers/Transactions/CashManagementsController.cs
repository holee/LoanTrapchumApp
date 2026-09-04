using Application.Features.CashManagements.Command.CreateCashVaultIn;
using Application.Features.CashManagements.Command.CreateCashVaultOut;
using Application.Features.CashManagements.UseCases;
using Application.Features.Transactions.CashManagements;
using Application.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.Transactions;

public class CashManagementsController(
    IMediator mediator,
    IGetCashVaultUseCase getCashVaultUseCase) : ApiControllerBase
{
    [HttpGet("vaults")]
    public async Task<IActionResult> GetCashVaults([FromQuery] int? branchId)
    {
        var result = await getCashVaultUseCase.ExecuteAsync(branchId);
        if (!result.IsSuccess)
        {
            return BadRequest(new { error = result.ErrMessage });
        }
        return Ok(result.Data);
    }

    [HttpPost("vault-in")]
    public async Task<IActionResult> StartCashVaultIn([FromBody] TrnxCashVaultDtoCreate request)
    {
        var result = await mediator.Send(new StartCashVaultIn(request));
        return HandleResult(result);
    }

    [HttpPost("vault-out")]
    public async Task<IActionResult> StartCashVaultOut([FromBody] TrnxCashVaultDtoCreate request)
    {
        var result = await mediator.Send(new StartCashVaultOut(request));
        return HandleResult(result);
    }
}
