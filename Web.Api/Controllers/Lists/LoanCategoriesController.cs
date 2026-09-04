using Application.Features.Categories.Requests;
using Application.Responses.Paginations;
using Application.Services.Loans;
using Application.Utils;
using Domains.Models.Bases;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.Lists;

public class LoanCategoriesController(ILoanCategoryServiceImpl loanCategoryService) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCategories(
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "LoanCategoryCode",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await loanCategoryService.GetCategoriesAsync(param);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetLoanCategory(int id)
    {
        var result = await loanCategoryService.GetLoanCategory(id);
        if (!result.IsSuccess)
        {
            return BadRequest(new { error = result.ErrMessage });
        }
        return Ok(result.Data);
    }

    [HttpGet("{id:int}/edit")]
    public async Task<IActionResult> GetForEdit(int id)
    {
        var result = await loanCategoryService.GetForEditAsync(id);
        if (!result.IsSuccess)
        {
            return BadRequest(new { error = result.ErrMessage });
        }
        return Ok(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] LoanCategoryDtoCreate request)
    {
        var result = await loanCategoryService.CreateAsync(request);
        if (!result.IsSuccess)
        {
            return BadRequest(new { error = result.ErrMessage });
        }
        return Ok(result.Data);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] LoanCategoryDtoUpdate request)
    {
        request.LoanCategoryId = id;
        var result = await loanCategoryService.UpdateAsync(request);
        if (!result.IsSuccess)
        {
            return BadRequest(new { error = result.ErrMessage });
        }
        return Ok(result.Data);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await loanCategoryService.DeleteAsync(id);
        if (!result.IsSuccess)
        {
            return BadRequest(new { error = result.ErrMessage });
        }
        return Ok(result.Data);
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(
        int id,
        [FromQuery] ChangeState state,
        CancellationToken cancellationToken)
    {
        var result = await loanCategoryService.UpdateStatusAsync(id, state, cancellationToken);
        if (!result.IsSuccess)
        {
            return BadRequest(new { error = result.ErrMessage });
        }
        return Ok(result.Data);
    }
}
