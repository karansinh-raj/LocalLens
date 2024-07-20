using System.Security.Claims;

namespace LocalLens.WebApi.DependencyInjection;

public static class ClaimPrincipalExtensions
{
    public static Guid GetUserId(
        this ClaimsPrincipal user)
    {
        var claim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (claim is not null && Guid.TryParse(claim.Value, out Guid UserId))
            return UserId;
        return Guid.Empty;
    }

    public static string? GetUserEmail(
        this ClaimsPrincipal user)
    {
        var claim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
        if (claim is not null)
            return claim.Value;
        return null;
    }
}
