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
    public static readonly ErrorResponse NullValue = new("Null Value");
    public static ErrorResponse Failure(string errorMessages) => new(errorMessages);
    public static ErrorResponse Failure(params List<string> errorMessages) => new(errorMessages);
    public static ErrorResponse Failure(string key, params List<string> errorMessages) => new(key, errorMessages);
    public ErrorResponse()
    {
        Errors = new Dictionary<string, List<string>>();
        Code = ErrorCode.Ok;
        Message = string.Empty;
    }
    public ErrorResponse(ErrorCode code, string errorMessages)
    {
        Errors?.Clear();
        Code = code;
        Message = errorMessages;
    }
    public ErrorResponse(string messages) => Errors?.Add("Message", [messages]);
    public ErrorResponse(params List<string> messages) => Errors?.Add("Message", messages);
    public ErrorResponse(string key, params List<string> messages) => Errors?.Add(key, messages);
    public ErrorCode Code { get; } = ErrorCode.Ok;
    public IDictionary<string, List<string>>? Errors { get; }
    public string Message { get; } = string.Empty;
    public static ErrorResponse NotFound(string errorMessages) => new(errorMessages);
    public static ErrorResponse NotFound(ErrorCode code, string errorMessages) => new(code, errorMessages);
    public static ErrorResponse Validation(string key, List<string> errorMessages) => new(key, errorMessages);
    public static ErrorResponse Forbiden(string errorMessages) => new(errorMessages);
    public static ErrorResponse Forbiden(ErrorCode code, string errorMessages) => new(code, errorMessages);
    public static ErrorResponse Unauthorized(string errorMessages) => new(errorMessages);
    public static ErrorResponse Unauthorized(ErrorCode code, string errorMessages) => new(code, errorMessages);
    public static ErrorResponse Conflict(string errorMessages) => new(errorMessages);
    public static ErrorResponse Conflict(ErrorCode code, string errorMessages) => new(code, errorMessages);
    public static ErrorResponse ServerError(string errorMessages) => new(errorMessages);
    public static ErrorResponse ServerError(ErrorCode code, string errorMessages) => new(code, errorMessages);

}
