namespace Applications.Commons.ExceptionHandlers;

public sealed class RequiredException : Exception
{
    public RequiredException(string message)
        : base(message)
    {

    }
}