namespace Applications.Responses
{
    public class CashStatusResult
    {
        public CashStatusResult()
        {
            IsOut = false; 
            Message = string.Empty; 
        }
        public bool IsOut { get; set; }
        public string Message { get; set; }
    }
}
