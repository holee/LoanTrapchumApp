using Domains.Models.Lists;

namespace Applications.Features.Holidays
{

    public static class HolidayCreateDtoExtensions
    {
        public static Holiday ToHoliday(this HolidayCreateDto source)
        {
            return new Holiday
            {
                Id = source.Id,
                HolidayDt = source.HolidayDt,
                HolidayName = source.HolidayName,
                HolidayNum = source.HolidayNum!.Value,
                Status = source.Status,
                Description = source.Description
            };
        }

        public static IEnumerable<Holiday> ToHolidays(this IEnumerable<HolidayCreateDto> source)
        {
            return source.Select(x => x.ToHoliday());
        }
    }

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
