namespace LocalLens.WebApi.ResultPattern;

public class Error
{
    private Error(
        string title,
        ErrorType errorType
    )
    {
        Title = title;
        ErrorType = errorType;
    }

    public string Title { get; set; }
    public ErrorType ErrorType { get; set; }

    public static Error Failure(string title) =>
        new(title, ErrorType.Failure);

    public static Error NotFound(string title) =>
        new(title, ErrorType.NotFound);

    public static Error Validation(string title) =>
        new(title, ErrorType.Validation);

    public static Error Conflict(string title) =>
        new(title, ErrorType.Conflict);

    public static Error AccessUnAuthorized(string title) =>
        new(title, ErrorType.AccessUnAuthorized);

    public static Error AccessForbidden(string title) =>
        new(title, ErrorType.AccessForbidden);

}
