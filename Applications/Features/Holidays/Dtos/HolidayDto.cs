using System.ComponentModel.DataAnnotations.Schema;

namespace Applications.Features.Holidays.Dtos
{

    public sealed class HolidayDto 
    {
        public long Id { get; set; }
        public DateTime? HolidayDt { get; set; }
        public string? HolidayName { get; set; } = string.Empty;
        public int HolidayNum { get; set; } = 0;
        public bool? Status { get; set; } = false;
        public string? Description { get; set; } = string.Empty;
        [NotMapped]public int RowIndex { get; set; } = 1;
    }
}
