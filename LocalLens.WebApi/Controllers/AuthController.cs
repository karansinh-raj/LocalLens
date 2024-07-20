using LocalLens.WebApi.Contracts.Auth;
using LocalLens.WebApi.DependencyInjection;
using LocalLens.WebApi.ResultPattern;
using LocalLens.WebApi.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocalLens.WebApi.Controllers;

[Route("auth")]
public class AuthController : BaseController
{
    private readonly IAuthService _authService;

    public AuthController(
        IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("google-login")]
    public async Task<IActionResult> GoogleLoginAsync(
        GoogleLoginRequest request, 
        CancellationToken ct)
    {
        var result = await _authService.GoogleLoginAsync(request, ct);

        return result.Match(
            Ok,
            Problem);
    }

    [HttpGet("user")]
    [Authorize]
    public async Task<IActionResult> GetProfileAsync(
        CancellationToken ct)
    {
        var userId = User.GetUserId();
        var result = await _authService.GetProfileAsync(userId, ct);

        return result.Match(
            Ok,
            Problem);
    }

    [HttpPut("user")]
    [Authorize]
    public async Task<IActionResult> UpdateProfileAsync(
        UpdateUserRequest request,
        CancellationToken ct)
    {
        var userId = User.GetUserId();
        var result = await _authService.UpdateProfileAsync(request, userId, ct);

        return result.Match(
            Ok,
            Problem);
    }

    [HttpDelete("user")]
    [Authorize]
    public async Task<IActionResult> DeleteProfileAsync(
        CancellationToken ct)
    {
        var userId = User.GetUserId();
        var result = await _authService.DeleteProfileAsync(userId, ct);

        return result.Match(
            Ok,
            Problem);
    }
}
