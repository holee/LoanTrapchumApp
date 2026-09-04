namespace Application.Features.Holidays.Dtos;

public static class HolidayExtensions
{
    public static HolidayDto ToHoliday(this Holiday holiday)
    {
        return new HolidayDto
        {
            Id = holiday.Id,
            HolidayDt = holiday.HolidayDt,
            HolidayName = holiday.HolidayName,
            HolidayNum = holiday.HolidayNum,
            Description = holiday.Description,
            Status = holiday.Status
        };
    }

    public static HolidayDtoUpdate ToHolidayUpdate(this Holiday holiday)
    {
        return new HolidayDtoUpdate
        {
            Id = holiday.Id,
            HolidayDt = holiday.HolidayDt,
            HolidayName = holiday.HolidayName,
            HolidayNum = holiday.HolidayNum,
            Description = holiday.Description,
            Status = holiday.Status
        };
    }


    public static IEnumerable<HolidayDto> ToHolidays(this IEnumerable<Holiday> holidays)
    {
        return holidays.Select(holiday => holiday.ToHoliday());
    }
}
