//namespace Applications.Commons.Results;

//public static class ResultExtensions
//{
//    public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> mapper)
//    {
//        return result.IsSuccess
//            ? Result<TOut>.Success(mapper(result.Value!))
//            : Result<TOut>.Failure(result.Errors);
//    }

//    public static Result<T> Ensure<T>(this Result<T> result, Func<T, bool> predicate, Error error)
//    {
//        if (result.IsFailure)
//            return result;

//        return predicate(result.Value!)
//            ? result
//            : Result<T>.Failure(error);
//    }

//    public static Result Combine(params Result[] results)
//    {
//        var failures = results.Where(r => r.IsFailure).SelectMany(r => r.Errors).ToList();

//        return failures.Any()
//            ? Result.Failure(failures)
//            : Result.Success();
//    }
//}
