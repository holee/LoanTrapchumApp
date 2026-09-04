namespace Web.UI.Helpers.Extensions
{
    public static class StringExtensions
    {
        public static (string First, string Last) WellFormName(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return (string.Empty, string.Empty);
            }

            var parts = source.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0)
            {
                return (string.Empty, string.Empty);
            }
            else if (parts.Length == 1)
            {
                return (parts[0], string.Empty);
            }
            else
            {
                // First name is first word
                var first = parts[0];
                // Last name is everything else joined back together (handles middle names)
                var last = string.Join(" ", parts.Skip(1));
                return (first, last);
            }
        }
    }
}
