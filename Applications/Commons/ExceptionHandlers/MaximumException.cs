namespace Applications.Commons.ExceptionHandlers;

public sealed class MaximumException : Exception
{
    public MaximumException(string message)  
        : base(message)
    {
    }
}
