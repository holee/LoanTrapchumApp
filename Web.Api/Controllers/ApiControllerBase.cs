using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult HandleResult<T>(ErrorOr<T> result)
    {
        if (result.IsError)
        {
            var firstError = result.FirstError;
            return firstError.Type switch
            {
                ErrorType.NotFound => NotFound(new { error = firstError.Description }),
                ErrorType.Validation => BadRequest(new { errors = result.Errors.Select(e => new { e.Code, e.Description }) }),
                ErrorType.Conflict => Conflict(new { error = firstError.Description }),
                ErrorType.Unauthorized => Unauthorized(new { error = firstError.Description }),
                ErrorType.Forbidden => Forbid(),
                _ => BadRequest(new { error = firstError.Description })
            };
        }

        return Ok(result.Value);
    }
}
