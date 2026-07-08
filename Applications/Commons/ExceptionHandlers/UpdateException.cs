namespace Applications.Commons.ExceptionHandlers;

public sealed class UpdateException : Exception
{
    public UpdateException()
        : base("An error occurred while attempting to update the resource.")
    {
    }

    public UpdateException(string message)
        : base(message)
    {
    }

    public UpdateException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
