namespace Application.Exceptions
{
    public sealed class ApplicationExceptions : Exception
    {
        public ApplicationExceptions() { }

        public ApplicationExceptions(string message) : base(message) { }
    }


}
