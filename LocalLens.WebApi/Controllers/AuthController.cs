using LocalLens.WebApi.Contracts.Auth;
using LocalLens.WebApi.Result;
using LocalLens.WebApi.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace LocalLens.WebApi.Controllers;

public class AuthController : BaseController
{
    private readonly IAuthService _authService;

    public AuthController(
        IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet("hi")]
    public async Task<IActionResult> Get(CancellationToken ct)
    {
        var result = await _authService.Get();
        return result.Match(Ok, Problem);
    }

    [HttpGet("hi1")]
    public async Task<IActionResult> Get1(CancellationToken ct)
    {
        var result = await _authService.Get1();
        return result.Match(Ok, Problem);
    }

    [HttpPost("hi1")]
    public async Task<IActionResult> Get1(GoogleLoginRequest request, CancellationToken ct)
    {
        var result = await _authService.Get1();
        return result.Match(Ok, Problem);
    }
}
