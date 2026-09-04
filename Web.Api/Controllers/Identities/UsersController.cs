using Application.Features.Identity.Command.ChangePassword;
using Application.Features.Identity.Command.CreateNewUser;
using Application.Features.Identity.Command.DeleteUser;
using Application.Features.Identity.Command.ToggleUserActive;
using Application.Features.Identity.Command.UpdateUserRole;
using Application.Features.Identity.Queries.GetUserById;
using Application.Features.Identity.Queries.GetUserDetailById;
using Application.Features.Identity.Queries.GetUserPermissions;
using Application.Features.Identity.Users;
using Application.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers.Identities;

public class UsersController(IMediator mediator) : ApiControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        var result = await mediator.Send(new GetUserByIdQuery(id));
        return HandleResult(result);
    }

    [HttpGet("{id}/details")]
    public async Task<IActionResult> GetUserDetailsById(string id)
    {
        var result = await mediator.Send(new GetUserDetailsByIdQuery(id));
        return HandleResult(result);
    }

    [HttpGet("{id}/permissions")]
    public async Task<IActionResult> GetUserPermissions(string id)
    {
        var result = await mediator.Send(new GetUserPermissionsQuery(id));
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] RegisterRequest request)
    {
        var result = await mediator.Send(new CreateNewUserCommand(request));
        return HandleResult(result);
    }

    [HttpPut("roles")]
    public async Task<IActionResult> UpdateUserRole([FromBody] UserRoleDtoUpdate request)
    {
        var result = await mediator.Send(new UpdateUserRoleCommand(request));
        return HandleResult(result);
    }

    [HttpPatch("{id}/toggle-active")]
    public async Task<IActionResult> ToggleUserActive(string id)
    {
        var result = await mediator.Send(new ToggleUserActiveCommand(id));
        return HandleResult(result);
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto request)
    {
        var result = await mediator.Send(new ChangePasswordCommand(request));
        return HandleResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var result = await mediator.Send(new DeleteUserCommand(id));
        return HandleResult(result);
    }
}
