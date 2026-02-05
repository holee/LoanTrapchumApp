namespace Applications.Bases;

public enum DeleteStatus
{
    Success,
    NotFound,
    InUse,
    Error
}

public sealed record DeleteResult
{
    public DeleteStatus Status { get; }
    public string? Message { get; } = string.Empty;

    private DeleteResult(DeleteStatus status, string? message)
    {
        Status = status;
        Message = message;
    }

    // ---------- Factory methods ----------
    public static DeleteResult Success(string? message = null) => new(DeleteStatus.Success, message);
    public static DeleteResult NotFound(string? message = null) => new(DeleteStatus.NotFound, message);
    public static DeleteResult InUse(string? message = null) => new(DeleteStatus.InUse, message);
    public static DeleteResult Error(string? message = null) => new(DeleteStatus.Error, message);

    // ---------- Convenience helpers ----------
    public bool IsSuccess => Status == DeleteStatus.Success;
    public bool IsFailure => Status != DeleteStatus.Success;

    public override string ToString()
        => Message is null ? Status.ToString() : $"{Status}: {Message}";
}
