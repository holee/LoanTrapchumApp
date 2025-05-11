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

    }
}
