using HRMSAPI.Constants;
using Microsoft.AspNetCore.Mvc;

namespace HRMSAPI.Controllers;

[ApiController]
public abstract class BaseController : ControllerBase
{
    protected IActionResult Success<T>(T data, string message = "Success")
    {
        return Ok(ApiResponse<T>.SuccessResponse(data, message));
    }

    protected IActionResult Error(string message, List<string>? errors = null)
    {
        return BadRequest(ApiResponse<object>.FailureResponse(message, errors));
    }

    protected IActionResult NotFoundResponse(string message)
    {
        return NotFound(ApiResponse<object>.FailureResponse(message));
    }
}