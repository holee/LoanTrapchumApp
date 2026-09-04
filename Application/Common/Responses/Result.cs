namespace Application.Responses;

public class Result
{
    public Result()
    {
        IsSuccess = false;
        Errors = ErrorResponse.None;
    }
    public Result(bool isSuccess)
    {
        IsSuccess = isSuccess;
        Errors = ErrorResponse.None;
    }
    public Result(bool isSuccess, ErrorResponse error) : this(isSuccess)
    {
        Errors = error;
    }
    public string ErrMessage => !IsSuccess ? Errors.ErrMessage : string.Empty;

    public IList<string> GetErrors
    {
        get
        {
            if (IsSuccess) return [];

            var errors = Errors.FieldErrors?
                               .Values.SelectMany(x => x)
                               .ToList() ?? [];

            return errors;
        }
    }
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
    public static Result<T> Failure<T>(T data) => new(false, ErrorResponse.None, data);
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
