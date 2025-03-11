namespace Domains.Models
{
    public class AppStatus
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string StatusOf { get; set; } = string.Empty;
    }
}
