using System.Runtime.CompilerServices;

namespace Application.Extensions.DateTimes;

public static class HolidayHelper
{

    public static void SetHoliday(List<DateTime> holidays,ref DateTime paymentDate)
    {
        foreach (var holiday in holidays)
        {
            if(IsSunday(paymentDate) || IsHoliday(paymentDate,holiday))
            {
                paymentDate=paymentDate.AddDays(1); 
            }
        }
    } 
    public static  bool IsSunday(DateTime date)
    {
        return date.DayOfWeek == DayOfWeek.Sunday;  
    }

    public static bool IsHoliday(DateTime payment,DateTime date)
    {
        var totalDays=(date - payment).TotalDays;
        return totalDays == 0;
    }



}
