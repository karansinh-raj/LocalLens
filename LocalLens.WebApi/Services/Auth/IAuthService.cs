using LocalLens.WebApi.Contracts.Auth;
using LocalLens.WebApi.ResultPattern;

namespace LocalLens.WebApi.Services.Auth;

public interface IAuthService : IBaseService
{
    Task<ResultT<GoogleLoginResponse>> GoogleLoginAsync(
        GoogleLoginRequest request,
        CancellationToken ct);

    Task<ResultT<string>> UpdateProfileAsync(
        UpdateUserRequest request,
        Guid userId,
        CancellationToken ct);

    Task<ResultT<string>> DeleteProfileAsync(
        Guid userId,
        CancellationToken ct);

    Task<ResultT<UserDetailsResponse>> GetProfileAsync(
        Guid userId,
        CancellationToken ct);
}
