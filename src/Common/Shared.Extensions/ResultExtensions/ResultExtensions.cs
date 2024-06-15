using FluentResults;

namespace Shared.Extensions.ResultExtensions;

public static class ResultExtensions
{
    public static Result<TIn> Tap<TIn>(this Result<TIn> result, Action<TIn> action)
    {
        if (result.IsSuccess)
        {
            action(result.Value);
        }

        return result;
    }

    public static async Task<Result<TIn>> Tap<TIn>(this Task<Result<TIn>> result, Action<TIn> action)
    {
        var resultAwaited = await result;
        if (resultAwaited.IsSuccess)
        {
            action(resultAwaited.Value);
        }

        return resultAwaited;
    }

    public static async Task<Result<TIn>> Tap<TIn>(this Task<Result<TIn>> result, Func<TIn, Task> action)
    {
        var resultAwaited = await result;
        if (resultAwaited.IsSuccess)
        {
            await action(resultAwaited.Value);
        }

        return resultAwaited;
    }
}