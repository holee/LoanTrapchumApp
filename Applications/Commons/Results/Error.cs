//namespace Applications.Commons.Results;

//public sealed record Error(string Code, string Message, ErrorType Type = ErrorType.Failure)
//{
//    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.None);

//    public static readonly Error Unexpected = new("General.Unexpected", "An unexpected error occurred.");
//    public override string ToString() => $"{Code}: {Message}";
//}