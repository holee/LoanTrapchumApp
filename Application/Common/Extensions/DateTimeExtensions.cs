namespace Application.Extensions;

public static class DateTimeExtensions
{

    public static string DayFirst(this DateTime dt)
    {
        return string.Format("{0:dd/MM/yyyy}", dt);
    }
    public static string DayFirst(this DateTimeOffset dt)
    {
        return string.Format("{0:dd/MM/yyyy}", dt);
    }
    public static string DayFirst(this DateTimeOffset? dt)
    {
        return string.Format("{0:dd/MM/yyyy}", dt);
    }
    public static string DayFirst(this DateTime? dt)
    {
        if (dt is null) return "-";

        return string.Format("{0:dd/MM/yyyy}", dt);
    }

    public static string DayName(this DateTime? dt)
    {
        var value = dt ?? DateTime.Now;

        return value.DayOfWeek.ToString();
    }

    public static string DayName(this DateTime dt)
    {
        return dt.DayOfWeek.ToString();
    }

    public static int DayNum(this DateTime dt)
    {
        return int.Parse(dt.ToString("yyyyMMdd"));
    }

    public static int DayNum(this DateTime? dt)
    {
        var value = dt ?? DateTime.Now;

        return int.Parse(value.ToString("yyyyMMdd"));
    }

    public static int DayNum(this DateTimeOffset? dt)
    {
        var value = dt ?? DateTime.Now;

        return int.Parse(value.ToString("yyyyMMdd"));
    }
    public static bool IsEqual(this DateTime? first, DateTime? second)
    {
        var _first = DateOnly.FromDateTime(first ?? DateTime.Now);

        var _second = DateOnly.FromDateTime(second ?? DateTime.Now);

        return _first.CompareTo(_second) == 0;
    }


    public static double Days(this DateTime? first, DateTime? second)
    {
        var _first = first ?? DateTime.Now;

        var _second = second ?? DateTime.Now;

        var timestampse = _second - _first;

        return timestampse.TotalDays;
    }

    public static bool IsRecently(this DateTime? current)
    {
        if (!current.HasValue)
            return false;
        var timeSince = DateTime.UtcNow - current.Value.ToUniversalTime();
        return timeSince.Minutes < 2;
    }

    public static bool IsRecently(this DateTimeOffset? current)
    {
        if (!current.HasValue)
            return false;
        return DateTimeOffset.UtcNow - current.Value <= TimeSpan.FromHours(23);
    }


    public static bool IsRecently(this DateTimeOffset? value, TimeSpan? threshold = null)
    {
        if (value is null)
            return false;

        var limit = threshold ?? TimeSpan.FromMinutes(2);

        return DateTimeOffset.UtcNow - value.Value <= limit;

    }
    public static long DateDiff(this DateTime start, DateTime end, DatePart part)
    {
        return part switch
        {
            DatePart.Year =>
                end.Year - start.Year,

            DatePart.Quarter =>
                ((end.Year - start.Year) * 12 + end.Month - start.Month) / 3,

            DatePart.Month =>
                (end.Year - start.Year) * 12L + end.Month - start.Month,

            DatePart.Week =>
                (long)((end - start).TotalDays / 7),

            DatePart.Day =>
                (long)(end - start).TotalDays,

            DatePart.Hour =>
                (long)(end - start).TotalHours,

            DatePart.Minute =>
                (long)(end - start).TotalMinutes,

            DatePart.Second =>
                (long)(end - start).TotalSeconds,

            DatePart.Millisecond =>
                (long)(end - start).TotalMilliseconds,

            DatePart.Tick =>
                end.Ticks - start.Ticks,

            _ => throw new ArgumentOutOfRangeException(nameof(part), part, null)
        };
    }

    public static long DateDiff(this DateTime? start, DateTime? end, DatePart part)
    {
        var startValue = start ?? DateTime.Now;

        var endValue = end ?? DateTime.Now;

        return part switch
        {
            DatePart.Year => endValue.Year - startValue.Year,

            DatePart.Quarter => ((endValue.Year - startValue.Year) * 12 + endValue.Month - startValue.Month) / 3,

            DatePart.Month => (endValue.Year - startValue.Year) * 12L + endValue.Month - startValue.Month,

            DatePart.Week => (long)((endValue - startValue).TotalDays / 7),

            DatePart.Day => (long)(endValue - startValue).TotalDays,

            DatePart.Hour => (long)(endValue - startValue).TotalHours,

            DatePart.Minute => (long)(endValue - startValue).TotalMinutes,

            DatePart.Second => (long)(endValue - startValue).TotalSeconds,

            DatePart.Millisecond => (long)(endValue - startValue).TotalMilliseconds,

            DatePart.Tick => endValue.Ticks - startValue.Ticks,

            _ => throw new ArgumentOutOfRangeException(nameof(part), part, null)
        };
    }


    public static long DateDiff(this DateTimeOffset? start, DateTimeOffset? end, DatePart part)
    {
        var startValue = start ?? DateTimeOffset.Now;

        var endValue = end ?? DateTimeOffset.Now;

        return part switch
        {
            DatePart.Year => endValue.Year - startValue.Year,

            DatePart.Quarter => ((endValue.Year - startValue.Year) * 12 + endValue.Month - startValue.Month) / 3,

            DatePart.Month => (endValue.Year - startValue.Year) * 12L + endValue.Month - startValue.Month,

            DatePart.Week => (long)((endValue - startValue).TotalDays / 7),

            DatePart.Day => (long)(endValue - startValue).TotalDays,

            DatePart.Hour => (long)(endValue - startValue).TotalHours,

            DatePart.Minute => (long)(endValue - startValue).TotalMinutes,

            DatePart.Second => (long)(endValue - startValue).TotalSeconds,

            DatePart.Millisecond => (long)(endValue - startValue).TotalMilliseconds,

            DatePart.Tick => endValue.Ticks - startValue.Ticks,

            _ => throw new ArgumentOutOfRangeException(nameof(part), part, null)
        };
    }



    public static int DaysUntil(this DateTime start, DateTime end) => (int)start.DateDiff(end, DatePart.Day);

    public static int MonthsUntil(this DateTime start, DateTime end) => (int)start.DateDiff(end, DatePart.Month);

    public static int YearsUntil(this DateTime start, DateTime end) => (int)start.DateDiff(end, DatePart.Year);

    public static int DaysUntil(this DateTime? start, DateTime? end) => (int)start.DateDiff(end, DatePart.Day);

    public static int MonthsUntil(this DateTime? start, DateTime? end) => (int)start.DateDiff(end, DatePart.Month);

    public static int YearsUntil(this DateTime? start, DateTime? end) => (int)start.DateDiff(end, DatePart.Year);

    public static int DaysUntil(this DateTimeOffset? start, DateTimeOffset? end) => (int)start.DateDiff(end, DatePart.Day);

    public static int MonthsUntil(this DateTimeOffset? start, DateTimeOffset? end) => (int)start.DateDiff(end, DatePart.Month);

    public static int YearsUntil(this DateTimeOffset? start, DateTimeOffset? end) => (int)start.DateDiff(end, DatePart.Year);


}
