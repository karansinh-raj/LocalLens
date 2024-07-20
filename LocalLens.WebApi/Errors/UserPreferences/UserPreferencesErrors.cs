using LocalLens.WebApi.ResultPattern;

namespace LocalLens.WebApi.Errors.UserPreferences;

public static class UserPreferencesErrors
{
    public static readonly Error UserNotFound =
        Error.NotFound("User not found");

    public static readonly Error UserPreferencesCreateFailure =
        Error.Validation("Something went wrong in creating user preferences");
}
