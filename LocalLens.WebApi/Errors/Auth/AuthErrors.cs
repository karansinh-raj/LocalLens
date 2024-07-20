using LocalLens.WebApi.ResultPattern;

namespace LocalLens.WebApi.Errors.Auth;

public static class AuthErrors
{
    public static readonly Error InvalidAccessToken =
        Error.AccessUnAuthorized("Invalid access token.");

    public static readonly Error UserCreateFailure =
       Error.Failure("Something went wrong in creating user");

    public static readonly Error UserNotFound =
      Error.NotFound("User not found");

    public static readonly Error UserUpdateFailure =
      Error.Failure("Something went wrong in updaing user");

    public static readonly Error UserDeleteFailure =
      Error.Failure("Something went wrong in deleing user");

    public static readonly Error UserAccountDeleted =
      Error.AccessUnAuthorized("User account does not exists or not active");
}
