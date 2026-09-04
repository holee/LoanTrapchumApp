namespace Application.Bases;

/// <summary>
/// Represents the outcome of a delete operation.
/// Use the factory methods to create instances and the helpers / Match overloads to consume them.
/// </summary>
public sealed record DeleteResult
{
    public DeleteStatus Status { get; }
    public string Message { get; }

    private DeleteResult(DeleteStatus status, string message)
    {
        Status = status;
        Message = message;
    }

    // ---------- Factory methods ----------

    public static DeleteResult Success(string message) => new(DeleteStatus.Success, message);
    public static DeleteResult NotFound(string message) => new(DeleteStatus.NotFound, message);
    public static DeleteResult InUse(string message) => new(DeleteStatus.InUse, message);
    public static DeleteResult Error(string message) => new(DeleteStatus.Error, message);

    // ---------- Status helpers ----------

    /// <summary>Record was found and successfully deleted.</summary>
    public bool IsSuccess => Status == DeleteStatus.Success;

    /// <summary>Any non-success outcome.</summary>
    public bool IsFailure => Status != DeleteStatus.Success;

    /// <summary>Record does not exist.</summary>
    public bool IsNotFound => Status == DeleteStatus.NotFound;

    /// <summary>Record is referenced by other data and must not be deleted.</summary>
    public bool IsInUse => Status == DeleteStatus.InUse;

    /// <summary>An unexpected / infrastructure error occurred.</summary>
    public bool IsError => Status == DeleteStatus.Error;

    // ---------- Functional match ----------

    /// <summary>
    /// Exhaustively maps every <see cref="DeleteStatus"/> to a value.
    /// Prefer this over a chain of if/switch blocks at call sites.
    /// </summary>
    public TResult Match<TResult>(
        Func<string, TResult> onSuccess,
        Func<string, TResult> onNotFound,
        Func<string, TResult> onInUse,
        Func<string, TResult> onError)
        => Status switch
        {
            DeleteStatus.Success => onSuccess(Message),
            DeleteStatus.NotFound => onNotFound(Message),
            DeleteStatus.InUse => onInUse(Message),
            DeleteStatus.Error => onError(Message),
            _ => throw new InvalidOperationException($"Unhandled {nameof(DeleteStatus)}: {Status}")
        };

    /// <summary>
    /// Exhaustively executes an action for every <see cref="DeleteStatus"/>.
    /// </summary>
    public void Switch(
        Action<string> onSuccess,
        Action<string> onNotFound,
        Action<string> onInUse,
        Action<string> onError)
    {
        switch (Status)
        {
            case DeleteStatus.Success: onSuccess(Message); break;
            case DeleteStatus.NotFound: onNotFound(Message); break;
            case DeleteStatus.InUse: onInUse(Message); break;
            case DeleteStatus.Error: onError(Message); break;
            default: throw new InvalidOperationException($"Unhandled {nameof(DeleteStatus)}: {Status}");
        }
    }

    // ---------- Deconstruct ----------

    public void Deconstruct(out DeleteStatus status, out string message)
        => (status, message) = (Status, Message);

    public override string ToString() => $"{Status}: {Message}";
}