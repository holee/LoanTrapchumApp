using System.Net;

namespace Applications.Commons.ExceptionHandlers;

public sealed class NotFoundException : Exception
{
    public NotFoundException(string message)
        : base(message)
    {
    }
}
