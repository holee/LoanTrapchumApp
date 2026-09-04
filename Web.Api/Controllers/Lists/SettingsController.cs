using Application.Features.Settings.Commands.AppSettings.CreateAppSetting;
using Application.Features.Settings.Commands.AppSettings.DeleteAppSetting;
using Application.Features.Settings.Commands.AppSettings.UpdateAppSetting;
using Application.Features.Settings.Commands.UserLoanSizes.CreateUserLoanSize;
using Application.Features.Settings.Commands.UserLoanSizes.DeleteUserLoanSize;
using Application.Features.Settings.Commands.UserLoanSizes.ToggleUserLoanSize;
using Application.Features.Settings.Commands.UserLoanSizes.UpdateUserLoanSize;
using Application.Features.Settings.Dto;
using Application.Features.Settings.Queries.AppSettings.GetAppSettingById;
using Application.Features.Settings.Queries.AppSettings.GetAppSettings;
using Application.Features.Settings.Queries.CollateralSubTypes.GetCollateralSubTypes;
using Application.Features.Settings.Queries.GetApprovedUsers;
using Application.Features.Settings.Queries.GetIncomeExpenses;
using Application.Features.Settings.Queries.GetLoanCategories;
using Application.Features.Settings.Queries.GetSystemSettings;
using Application.Features.Settings.Queries.UserLoanSizes.GetLoanSizeByUser;
using Application.Features.Settings.Queries.UserLoanSizes.GetUserLoanSizeById;
using Application.Features.Settings.Queries.UserLoanSizes.GetUserLoanSizes;
using Application.Responses.Paginations;
using Application.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.Lists;

public class SettingsController(IMediator mediator) : ApiControllerBase
{
    // App Settings
    [HttpGet("app-settings")]
    public async Task<IActionResult> GetAppSettings([FromQuery] string? filter = null)
    {
        var result = await mediator.Send(new GetAppSettingsQuery(filter));
        return HandleResult(result);
    }

    [HttpGet("app-settings/{id:int}")]
    public async Task<IActionResult> GetAppSettingById(int id)
    {
        var result = await mediator.Send(new GetAppSettingByIdQuery(id));
        return HandleResult(result);
    }

    [HttpPost("app-settings")]
    public async Task<IActionResult> CreateAppSetting([FromBody] AppSettingDtoCreate request)
    {
        var result = await mediator.Send(new CreateAppSettingCommand(request));
        return HandleResult(result);
    }

    [HttpPut("app-settings/{id:int}")]
    public async Task<IActionResult> UpdateAppSetting(int id, [FromBody] AppSettingDtoUpdate request)
    {
        request.Id = id;
        var result = await mediator.Send(new UpdateAppSettingCommand(request));
        return HandleResult(result);
    }

    [HttpDelete("app-settings/{id:int}")]
    public async Task<IActionResult> DeleteAppSetting(int id)
    {
        var result = await mediator.Send(new DeleteAppSettingCommand(id));
        return HandleResult(result);
    }

    // System Settings
    [HttpGet("system-settings")]
    public async Task<IActionResult> GetSystemSettings(
        [FromQuery] int? id = null,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "SettingKey",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetSystemSettingQuery(id, param));
        return Ok(result);
    }

    // Loan Categories
    [HttpGet("loan-categories")]
    public async Task<IActionResult> GetLoanCategories(
        [FromQuery] int? id = null,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "CategoryName",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetLoanCategoriesQuery(id, param));
        return Ok(result);
    }

    // Collateral Sub Types
    [HttpGet("collateral-sub-types")]
    public async Task<IActionResult> GetCollateralSubTypes(
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "SubTypeName",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetCollateralSubTypesQuery(param));
        return Ok(result);
    }

    // Approved Users
    [HttpGet("approved-users")]
    public async Task<IActionResult> GetApprovedUsers(
        [FromQuery] int? branchId = null,
        [FromQuery] int? currencyId = null,
        [FromQuery] decimal? amount = null)
    {
        var result = await mediator.Send(new GetApprovedUserQuery(branchId, currencyId, amount, null));
        return HandleResult(result);
    }

    // Income Expense By Id
    [HttpGet("income-expense/{id:int}")]
    public async Task<IActionResult> GetIncomeExpenseById(int id)
    {
        var result = await mediator.Send(new GetIncomeExpenseByIdQuery(id));
        return HandleResult(result);
    }

    // User Loan Sizes
    [HttpGet("user-loan-sizes")]
    public async Task<IActionResult> GetUserLoanSizes(
        [FromQuery] string? userRefId = null,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "CreatedAt",
        [FromQuery] bool isDescending = true,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetUserLoanSizesQuery(userRefId, param));
        return Ok(result);
    }

    [HttpGet("user-loan-sizes/{id:long}")]
    public async Task<IActionResult> GetUserLoanSizeById(long id)
    {
        var result = await mediator.Send(new GetUserLoanSizeByIdQuery(id));
        return HandleResult(result);
    }

    [HttpGet("user-loan-sizes/by-user")]
    public async Task<IActionResult> GetLoanSizeByUser([FromQuery] string? userId = null)
    {
        var result = await mediator.Send(new GetLoanSizeByUserQuery(userId));
        return HandleResult(result);
    }

    [HttpPost("user-loan-sizes")]
    public async Task<IActionResult> CreateUserLoanSize([FromBody] UserLoanSizeDtoCreate request)
    {
        var result = await mediator.Send(new CreateUserLoanSizeCommand(request));
        return HandleResult(result);
    }

    [HttpPut("user-loan-sizes/{id:long}")]
    public async Task<IActionResult> UpdateUserLoanSize(long id, [FromBody] UserLoanSizeDtoUpdate request)
    {
        request.Id = id;
        var result = await mediator.Send(new UpdateUserLoanSizeCommand(request));
        return HandleResult(result);
    }

    [HttpDelete("user-loan-sizes/{id:long}")]
    public async Task<IActionResult> DeleteUserLoanSize(long id)
    {
        var result = await mediator.Send(new DeleteUserLoanSizeCommand(id));
        return HandleResult(result);
    }

    [HttpPatch("user-loan-sizes/{id:long}/toggle")]
    public async Task<IActionResult> ToggleUserLoanSize(long id, [FromQuery] string userRefId)
    {
        var result = await mediator.Send(new ToggleUserLoanSizeCommand(id, userRefId));
        return HandleResult(result);
    }
}
