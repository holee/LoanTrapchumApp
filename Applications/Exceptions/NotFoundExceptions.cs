namespace Applications.Exceptions
{
    public sealed class NotFoundExceptions : Exception
    {
        public NotFoundExceptions() { }
        public NotFoundExceptions(string message) : base(message) { }

    }


}
