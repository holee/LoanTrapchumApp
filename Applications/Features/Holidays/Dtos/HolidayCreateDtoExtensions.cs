namespace Applications.Features.Holidays.Dtos
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

        public static List<Holiday> ToHolidays(this List<HolidayCreateDto> source)
        {
            return source.Select(x => x.ToHoliday()).ToList();
        }
    }
}
