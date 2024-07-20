using LocalLens.WebApi.Contracts.Auth;
using LocalLens.WebApi.ResultPattern;

namespace LocalLens.WebApi.Services.Auth;

public interface IAuthService : IBaseService
{
    Task<ResultT<GoogleLoginResponse>> GoogleLoginAsync(
        GoogleLoginRequest request,
        CancellationToken ct);
}
