using HRMSAPI.Constants;
using HRMSAPI.DTOs;
using HRMSAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMSAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : BaseController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);
        if (response == null)
        {
            return Unauthorized(ApiResponse<object>.FailureResponse("Invalid credentials"));
        }

        return Ok(ApiResponse<AuthResponse>.SuccessResponse(response, "Login successful"));
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await _authService.RegisterAsync(request);
        if (!result)
        {
            return BadRequest(ApiResponse<object>.FailureResponse("User registration failed. Email might already exist."));
        }

        return Ok(ApiResponse<object>.SuccessResponse(null, "User registered successfully"));
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] TokenRequest request)
    {
        var response = await _authService.RefreshTokenAsync(request);
        if (response == null)
        {
            return BadRequest(ApiResponse<object>.FailureResponse("Invalid refresh token"));
        }

        return Ok(ApiResponse<AuthResponse>.SuccessResponse(response, "Token refreshed successfully"));
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
    {
        var result = await _authService.LogoutAsync(request.RefreshToken);
        if (!result)
        {
            return BadRequest(ApiResponse<object>.FailureResponse("Invalid or already revoked refresh token"));
        }

        return Ok(ApiResponse<object>.SuccessResponse(null, "Logged out successfully"));
    }
}