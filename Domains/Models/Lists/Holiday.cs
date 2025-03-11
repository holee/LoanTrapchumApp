namespace Domains.Models.Lists
{
    public class Holiday
    {
        public long Id { get; set; }
        public DateTime HolidayName { get; set; }
        public bool? Status { get; set; }
        public string? Description { get; set; } = string.Empty;
    }
}

