namespace Web.UI.Helpers.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTimeOffset ToDateTimeOffset(this DateTime dt, TimeSpan? offset = null)
        {
            return offset.HasValue
                ? new DateTimeOffset(dt, offset.Value)
                : new DateTimeOffset(dt, TimeZoneInfo.Local.GetUtcOffset(dt));
        }

    }
}
