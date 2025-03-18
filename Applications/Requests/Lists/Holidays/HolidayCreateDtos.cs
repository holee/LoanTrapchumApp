using System.ComponentModel.DataAnnotations;

namespace Applications.Requests.Lists.Holidays
{
    public class HolidayCreateDto
    {
        public long Id { get; set; } 
        public DateTime? HolidayName { get; set; }
        public bool? Status { get; set; }=true;
        public string? Description { get; set; } = string.Empty;
    }
}
