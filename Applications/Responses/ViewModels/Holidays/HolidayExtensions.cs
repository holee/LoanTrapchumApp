using Applications.Features.Holidays;

namespace Applications.Responses.ViewModels.Holidays
{
    public static class HolidayExtensions
    {
        public static HolidayVm ToHoliday(this Holiday holiday)
        {
            return new HolidayVm
            {
                Id = holiday.Id,
                HolidayDt = holiday.HolidayDt,
                HolidayName = holiday.HolidayName,
                HolidayNum = holiday.HolidayNum,
                Description = holiday.Description,
                Status = holiday.Status
            };
        }


        public static HolidayCreateDto ToHolidayCreateOrEdit(this Holiday holiday)
        {
            return new HolidayCreateDto
            {
                Id = holiday.Id,
                HolidayDt = holiday.HolidayDt,
                HolidayName = holiday.HolidayName,
                HolidayNum = holiday.HolidayNum,
                Description = holiday.Description,
                Status = holiday.Status
            };
        }

        public static IEnumerable<HolidayVm> ToHolidays(this IEnumerable<Holiday> holidays)
        {
            return holidays.Select(holiday => holiday.ToHoliday());
        }
    }
}
