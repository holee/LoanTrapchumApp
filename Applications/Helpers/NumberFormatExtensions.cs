namespace Applications.Helpers
{
    public static class NumberFormatExtensions
    {

        public static string Format(this decimal? d, string formatter = "N2")
        {
            var formatStringValue = d ?? 0;

            return string.Format($"{{0:{formatter}}}", formatStringValue);

        }

        public static string Format(this decimal d, string formatter = "N2")
        {
            var formatStringValue = d;
            return string.Format($"{{0:{formatter}}}", formatStringValue);

        }



    }
}
