namespace Applications.Commons.ExceptionHandlers;

public sealed class DuplicateException : Exception
{
    public DuplicateException(string message) 
        : base(message)
    {
    }
}
