namespace Applications.Responses;

public enum ErrorCode
{
    NullValue = 0,
    Ok = 200,
    Validation = 400,
    NotFound = 404,
    Forbiden = 403,
    Unauthorized = 401,
    Conflict = 409,
    ServerError = 500,
    ServiceUnavailable = 503
}
public sealed record ErrorResponse
{
    public static readonly ErrorResponse None = new();

    public static readonly ErrorResponse NullValue = new(ErrorCode.NullValue, "Null Value");
    public static ErrorResponse Failure(string errorMessages) => new(errorMessages);
    public ErrorResponse()
    {
        Code = ErrorCode.NullValue;
        ErrMessage = string.Empty;
    }
    public ErrorResponse(ErrorCode code, string errorMessages)
    {
        Code = code;
        ErrMessage = errorMessages;
    }
    public ErrorResponse(ErrorCode code, IDictionary<string, string[]>? fieldErrors)
    {
        Code = code;
        FieldErrors = fieldErrors;
    }
    public ErrorResponse(string errMessages) => ErrMessage = errMessages;
    public ErrorCode Code { get; } = ErrorCode.Ok;
    public string ErrMessage { get; } = string.Empty;
    public IDictionary<string, string[]>? FieldErrors { get; set; }
    public bool HasErrors => FieldErrors!.Any();
    public static ErrorResponse Validation(string errorMessages) => new(ErrorCode.Validation, errorMessages);
    public static ErrorResponse Validation(IDictionary<string, string[]>? fieldErrors) => new(ErrorCode.Validation, fieldErrors);
    public static ErrorResponse NotFound(string errorMessages) => new(ErrorCode.NotFound, errorMessages);
    public static ErrorResponse Forbiden(string errorMessages) => new(ErrorCode.Forbiden, errorMessages);
    public static ErrorResponse Unauthorized(string errorMessages) => new(ErrorCode.Unauthorized, errorMessages);
    public static ErrorResponse Conflict(string errorMessages) => new(ErrorCode.Conflict, errorMessages);
    public static ErrorResponse ServerError(string errorMessages) => new(ErrorCode.ServerError, errorMessages);

}
