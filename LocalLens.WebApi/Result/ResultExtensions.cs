namespace LocalLens.WebApi.Result;

public static class ResultExtensions
{
    public static T Match<T>(
        this Result result,
        Func<T> onSuccess,
        Func<Error, T> onFailure)
    {
        return result.IsSuccess ? onSuccess() : onFailure(result.Error!);
    }

    public static T Match<T, TValue>(
        this ResultT<TValue> result,
        Func<ResultT<TValue>, T> onSuccess,
        Func<Error, T> onFailure)
    {
        return result.IsSuccess ? onSuccess(result) : onFailure(result.Error!);
    }
}
