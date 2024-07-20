using LocalLens.WebApi.ResultPattern;

namespace LocalLens.WebApi.Errors.UserQuestions;

public class UserQuestionsErrors
{
    public static readonly Error UserNotFound =
        Error.NotFound("User not found");

    public static readonly Error UserQuestionsCreateFailure =
        Error.Validation("Something went wrong in creating user questions");
}
