namespace Application.Commons.ExceptionHandlers;

public sealed class ScheduleAlreadyPaidException : Exception
{
    public ScheduleAlreadyPaidException()
        : base("The schedule has already been paid.")
    {
    }

    public ScheduleAlreadyPaidException(string message)
        : base(message)
    {
    }

    public ScheduleAlreadyPaidException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
