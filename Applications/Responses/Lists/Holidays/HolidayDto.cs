namespace Applications.Responses.Lists.Holidays;

public class HolidayDto
{
    public long Id { get; set; }
    public DateTime? HolidayDt { get; set; }
    public string? HolidayName { get; set; } = string.Empty;
    public int HolidayNum { get; set; } = 0;
    public bool? Status { get; set; }
    public string? Description { get; set; } = string.Empty;
}
