namespace Applications.Commons.ExceptionHandlers;

public sealed class MinimumException : Exception
{
    public MinimumException(string message)
        : base(message)
    {
    }
}