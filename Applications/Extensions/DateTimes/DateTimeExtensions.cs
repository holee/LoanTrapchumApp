namespace Applications.Extensions.DateTimes
{
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

            return _first.CompareTo(_second)==0;
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
    }
}
