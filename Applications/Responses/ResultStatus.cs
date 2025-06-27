namespace Applications.Responses;

public sealed class ResultStatus
{
    public ResultStatus()
    {
        IsOpen = false;
        Message = string.Empty;
    }

    public ResultStatus(bool isOpen, string message) : base()
    {
        IsOpen = isOpen;
        Message = message;
    }

    public bool IsOpen { get; set; }
    public string Message { get; set; }
}
