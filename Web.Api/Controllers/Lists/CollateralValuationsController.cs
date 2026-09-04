using Applications.Features.CollateralValuations.Dtos;
using Applications.Interfaces;
using Applications.Responses.Paginations;
using Domains.Models.Collaterals;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.Lists;

public class CollateralValuationsController(ICollateralValuationService service) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCollateralValuations(
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "Id",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null,
        CancellationToken cancellationToken = default)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await service.GetAsPagingAsync(param, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetCollateralValuationById(long id, CancellationToken cancellationToken = default)
    {
        var result = await service.GetByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCollateralValuation([FromBody] CollateralValuationDtoCreate request, CancellationToken cancellationToken = default)
    {
        var entity = request.Adapt<CollateralEvaluations>();
        entity.CreatedDT = DateTimeOffset.UtcNow;
        entity.ModifiedDT = DateTimeOffset.UtcNow;
        var result = await service.CreateAsync(entity, cancellationToken);
        return HandleResult(result);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateCollateralValuation(long id, [FromBody] CollateralValuationDtoUpdate request, CancellationToken cancellationToken = default)
    {
        request.Id = id;
        var entity = request.Adapt<CollateralEvaluations>();
        entity.ModifiedDT = DateTimeOffset.UtcNow;
        var result = await service.UpdateAsync(entity, cancellationToken);
        return HandleResult(result);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteCollateralValuation(long id, CancellationToken cancellationToken = default)
    {
        var result = await service.DeleteAsync(id, cancellationToken);
        return HandleResult(result);
    }
}
