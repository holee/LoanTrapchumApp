namespace Application.Features.Holidays.Dtos
{
    public class HolidayCreateDto
    {
        public long Id { get; set; }
        public DateTime? HolidayDt { get; set; }
        public string? HolidayName { get; set; }
        public int? HolidayNum { get; set; }
        public bool? Status { get; set; } = true;
        public string? Description { get; set; } = string.Empty;
    }
}
