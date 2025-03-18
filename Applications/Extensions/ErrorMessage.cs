namespace Applications.Extensions
{
    public class ErrorMessage
    {
        public string? Message { get; set; } = string.Empty;
        public string[] Errors { get; set; } = Array.Empty<string>();
    }
}
