using System.Text.Json.Serialization;

namespace LocalLens.WebApi.ResultPattern;

public class Result
{
    protected Result()
    {
        IsSuccess = true;
        Status = 200;
        Error = default;
    }

    protected Result(Error error)
    {
        IsSuccess = false;
        Error = error;
    }

    public bool IsSuccess { get; }

    public int Status { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Error? Error { get; }

    public static implicit operator Result(Error error) =>
       new(error);

    public static Result Success() =>
        new();
    public static Result Failure(Error error) =>
        new(error);
}
