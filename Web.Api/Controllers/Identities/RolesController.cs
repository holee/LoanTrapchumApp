using Application.Features.Identity.Roles.Commands.AddRolePermissions;
using Application.Features.Identity.Roles.Commands.CreateRole;
using Application.Features.Identity.Roles.Commands.DeleteRole;
using Application.Features.Identity.Roles.Commands.UpdateRole;
using Application.Features.Identity.Roles.Queries.EditRole;
using Application.Features.Identity.Roles.Queries.GetAllPermissions;
using Application.Features.Identity.Roles.Queries.GetPermissionsByRole;
using Application.Features.Identity.Roles.Queries.GetRoles;
using Application.Features.Identity.Roles.Queries.GetRolesAsPaging;
using Application.Features.Identity.Roles.Requests;
using Application.Features.Identity.Users;
using Application.Responses.Paginations;
using Application.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.Identities;

public class RolesController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetRoles(
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 15,
        [FromQuery] string? sortColumn = "Name",
        [FromQuery] bool isDescending = false,
        [FromQuery] string? filter = null)
    {
        var param = new QueryParam(pageIndex, pageSize, sortColumn, isDescending, false, filter);
        var result = await mediator.Send(new GetRolesAsPagingQuery(param));
        return Ok(result);
    }

    [HttpGet("names")]
    public async Task<IActionResult> GetRoleNames([FromQuery] string? filter = null)
    {
        var result = await mediator.Send(new GetRolesQuery(filter));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoleById(string id)
    {
        var result = await mediator.Send(new EditRoleQuery(id));
        return HandleResult(result);
    }

    [HttpGet("permissions")]
    public async Task<IActionResult> GetAllPermissions()
    {
        var result = await mediator.Send(new GetAllPermissionsQuery());
        return Ok(result);
    }

    [HttpGet("{id}/permissions")]
    public async Task<IActionResult> GetPermissionsByRole(string id)
    {
        var result = await mediator.Send(new GetPermissionsByRoleQuery(id));
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] RoleCreateDto request)
    {
        var result = await mediator.Send(new CreateRoleCommand(request));
        return HandleResult(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRole(string id, [FromBody] RoleDtoUpdate request)
    {
        request.RoleId = id;
        var result = await mediator.Send(new UpdateRoleCommand(request));
        return HandleResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRole(string id)
    {
        var result = await mediator.Send(new DeleteRoleCommand(id));
        return HandleResult(result);
    }

    [HttpPost("permissions")]
    public async Task<IActionResult> AddRolePermissions([FromBody] RolePermissionRequest request)
    {
        var result = await mediator.Send(new AddRolePermissionsCommand(request));
        return HandleResult(result);
    }
}
