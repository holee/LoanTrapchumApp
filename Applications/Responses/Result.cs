using Microsoft.AspNetCore.SignalR;

namespace Applications.Responses;

public class Result
{
    public Result()
    {
        IsSuccess = false;
        Errors = ErrorResponse.None;
    }
    public Result(bool isSuccess, ErrorResponse error)
    {
        IsSuccess = isSuccess;
        Errors = error;
    }

    public bool HasError(string? key, out string message)
    {
        message = string.Empty;

        if (!HasError(key))
        {
            return false;
        }

        if (!string.IsNullOrEmpty(key) && Errors.Errors!.ContainsKey(key))
        {
            message = Errors!.Errors[key]!.First();
            return true;
        }

        var errors = Errors.Errors?.Values.FirstOrDefault();

        if (errors!.Any())
        {
            message = errors!.First();
            return true;
        }

        return false;



    }
    private bool HasError(string? key = null)
    {
        if (string.IsNullOrEmpty(key))
        {
            return Errors.Errors!.Any();
        }
        return !IsSuccess && Errors.Errors!.ContainsKey(key);
    }
    public string ErrMessage => !IsSuccess ? Errors.Message : string.Empty;
    public bool IsSuccess { get; }
    public ErrorResponse Errors { get; }
    public static Result<T> Success<T>(T data) => new(true, ErrorResponse.None, data);
    public static Task<Result<T>> SuccessAsync<T>(T data) => Task.FromResult(Success(data));
    public static Result Success() => new(true, ErrorResponse.None);
    public static Task<Result> SuccessAsync() => Task.FromResult(Success());
    public static Result Failure() => new(false, ErrorResponse.None);
    public static Result Failure(ErrorResponse error) => new(false, error);
    public static Task<Result> FailureAsyn(ErrorResponse error) => Task.FromResult(Failure(error));
    public static Result<T> Failure<T>(ErrorResponse error) => new(false, error, default);
    public static Task<Result<T>> FailureAsync<T>(ErrorResponse error) => Task.FromResult(Failure<T>(error));
}

public sealed class Result<T> : Result
{
    public T? Data { get; }
    public Result(bool isSuccess, ErrorResponse error, T? data) : base(isSuccess, error)
    {
        Data = data;
    }


}
