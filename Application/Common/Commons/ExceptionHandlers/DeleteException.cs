namespace Application.Commons.ExceptionHandlers;

public sealed class DeleteException : Exception
{
    public DeleteException()
        : base("An error occurred while attempting to delete the resource.")
    {
    }

    public DeleteException(string message)
        : base(message)
    {
    }

    public DeleteException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}