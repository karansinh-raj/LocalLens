using LocalLens.WebApi.Contracts.Auth;
using LocalLens.WebApi.ResultPattern;
using LocalLens.WebApi.Services.Auth;
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
}
