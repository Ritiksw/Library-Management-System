using LibraryManagementSystem.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Server.Controllers;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected ActionResult ToActionResult(ServiceResult result)
    {
        if (result.Success) return NoContent();

        return result.ErrorType switch
        {
            ServiceErrorType.NotFound => NotFound(new { message = result.ErrorMessage }),
            ServiceErrorType.Conflict => Conflict(new { message = result.ErrorMessage }),
            ServiceErrorType.BadRequest => BadRequest(new { message = result.ErrorMessage }),
            _ => StatusCode(500)
        };
    }

    protected ActionResult<T> ToActionResult<T>(ServiceResult<T> result)
    {
        if (result.Success) return Ok(result.Data);

        return result.ErrorType switch
        {
            ServiceErrorType.NotFound => NotFound(new { message = result.ErrorMessage }),
            ServiceErrorType.Conflict => Conflict(new { message = result.ErrorMessage }),
            ServiceErrorType.BadRequest => BadRequest(new { message = result.ErrorMessage }),
            _ => StatusCode(500)
        };
    }
}
