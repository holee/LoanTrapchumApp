namespace Applications.Bases
{
    public enum DeleteStatus
    {
        Success,
        NotFound,
        InUse,
        Error
    }


    public sealed record DeleteResult(DeleteStatus Status, string? Message)
    {
        public static DeleteResult Success(string? message = null) => new(DeleteStatus.Success, message);
        public static DeleteResult NotFound(string? message = null) => new(DeleteStatus.NotFound, message);
        public static DeleteResult InUse(string? message = null) => new(DeleteStatus.InUse, message);
        public static DeleteResult Error(string? message = null) => new(DeleteStatus.Error, message);
    }
}
