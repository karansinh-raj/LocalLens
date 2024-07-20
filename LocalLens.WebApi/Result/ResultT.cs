namespace LocalLens.WebApi.Result;

public class ResultT<TValue> : Result
{
    private readonly TValue? _value;
    private readonly string? _title;

    private ResultT(
        TValue value, string? title) : base()
    {
        _value = value;
        _title = title;
    }

    private ResultT(
       Error error) : base(error)
    {
        _value = default;
    }

    public TValue? Data => _value;
    public string? Title => _title;

    public static implicit operator ResultT<TValue>(Error error) =>
        new(error);

    public static implicit operator ResultT<TValue>((TValue value, string? title) tuple) =>
        new(tuple.value, tuple.title);

    public static ResultT<TValue> Success(TValue value, string? title) =>
        new(value, title);

    public static new ResultT<TValue> Failure(Error error) =>
        new(error);
}
