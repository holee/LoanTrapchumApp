using Applications.Features.Items.Dto;
using Applications.Interfaces.Lists;
using Applications.Responses.Paginations;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.Lists;

public class ItemsController(IItemService service) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetItems(
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "Id",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null,
        CancellationToken cancellationToken = default)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await service.GetItemsAsync(param, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetItemById(int id, CancellationToken cancellationToken = default)
    {
        var result = await service.GetItemByIdAsync(id, cancellationToken);
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateItem([FromBody] ItemDtoCreate request, CancellationToken cancellationToken = default)
    {
        var result = await service.CreateAsync(request, cancellationToken);
        return HandleResult(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateItem(int id, [FromBody] ItemDtoUpdate request, CancellationToken cancellationToken = default)
    {
        request.Id = id;
        var result = await service.UpdateAsync(request, cancellationToken);
        return HandleResult(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteItem(int id, CancellationToken cancellationToken = default)
    {
        var result = await service.DeleteAsync(id, cancellationToken);
        return HandleResult(result);
    }
}
