namespace Domains.Models.Lists
{
    public sealed class Holiday
    {
        public long Id { get; set; }
        public DateTime? HolidayDt { get; set; }
        public string? HolidayName { get; set; } = string.Empty;
        public int HolidayNum { get; set; } = 0;
        public bool? Status { get; set; } = false;
        public string? Description { get; set; } = string.Empty;
    }


   
}

