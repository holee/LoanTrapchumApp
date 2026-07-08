namespace Applications.Bases;

public sealed record AppResult
{
    public ResultState Status { get; }
    public string Message { get; }

    private AppResult(ResultState status, string message)
    {
        Status = status;
        Message = message;
    }

    // ---------- Factory methods ----------

    public static AppResult Success(string message) => new(ResultState.Success, message);
    public static AppResult NotFound(string message) => new(ResultState.NotFound, message);
    public static AppResult InUse(string message) => new(ResultState.InUse, message);
    public static AppResult Error(string message) => new(ResultState.Error, message);

    // ---------- Status helpers ----------

    /// <summary>Record was found and successfully deleted.</summary>
    public bool IsSuccess => Status == ResultState.Success;

    /// <summary>Any non-success outcome.</summary>
    public bool IsFailure => Status != ResultState.Success;

    /// <summary>Record does not exist.</summary>
    public bool IsNotFound => Status == ResultState.NotFound;

    /// <summary>Record is referenced by other data and must not be deleted.</summary>
    public bool IsInUse => Status == ResultState.InUse;

    /// <summary>An unexpected / infrastructure error occurred.</summary>
    public bool IsError => Status == ResultState.Error;

    // ---------- Functional match ----------

    /// <summary>
    /// Exhaustively maps every <see cref="ResultState"/> to a value.
    /// Prefer this over a chain of if/switch blocks at call sites.
    /// </summary>
    public TResult Match<TResult>(
        Func<string, TResult> onSuccess,
        Func<string, TResult> onNotFound,
        Func<string, TResult> onInUse,
        Func<string, TResult> onError)
        => Status switch
        {
            ResultState.Success => onSuccess(Message),
            ResultState.NotFound => onNotFound(Message),
            ResultState.InUse => onInUse(Message),
            ResultState.Error => onError(Message),
            _ => throw new InvalidOperationException($"Unhandled {nameof(ResultState)}: {Status}")
        };

    /// <summary>
    /// Exhaustively executes an action for every <see cref="ResultState"/>.
    /// </summary>
    public void Switch(
        Action<string> onSuccess,
        Action<string> onNotFound,
        Action<string> onInUse,
        Action<string> onError)
    {
        switch (Status)
        {
            case ResultState.Success: onSuccess(Message); break;
            case ResultState.NotFound: onNotFound(Message); break;
            case ResultState.InUse: onInUse(Message); break;
            case ResultState.Error: onError(Message); break;
            default: throw new InvalidOperationException($"Unhandled {nameof(ResultState)}: {Status}");
        }
    }

    // ---------- Deconstruct ----------

    public void Deconstruct(out ResultState status, out string message) => (status, message) = (Status, Message);

    public override string ToString() => $"{Status}: {Message}";
}