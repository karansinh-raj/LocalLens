using LocalLens.WebApi.ResultPattern;

namespace LocalLens.WebApi.Errors.Auth;

public static class AuthErrors
{
    public static readonly Error InvalidAccessToken =
        Error.AccessUnAuthorized("Invalid access token.");

    public static readonly Error UserCreateFailure =
       Error.Failure("Something went wrong in creating user");
}
