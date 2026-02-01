namespace TrapchumApp.Helpers.Extensions
{
    public static class LoadDataArgsExtensions
    {

        public static (string, bool) PageInfo(this LoadDataArgs args, string? sortColumns = null)
        {
            if (args.Sorts!.Any())
            {
                var sortField = args.Sorts!.FirstOrDefault()?.Property ?? "Id";

                var sortDescending = args.Sorts!.FirstOrDefault()?.SortOrder == SortOrder.Ascending;

                return (sortField, sortDescending);

            }

            return (sortColumns ?? "", false);
        }


        public static (string FilterProperty, T? FilterValue) DataArgs<T>(this LoadDataArgs args)
        {

            if (!args.Filters!.Any()) return (string.Empty, default(T));

            var filterDescriptor = args.Filters?.FirstOrDefault();

            var filterProperty = filterDescriptor?.Property;

            var filterValue = filterDescriptor?.FilterValue;

            return (filterProperty ?? string.Empty, (T?)filterValue);

        }



        public static T? GetFilterValue<T>(this LoadDataArgs e, params string[] properties)
        {
            var value = e.Filters?
                         .FirstOrDefault(f => properties.Contains(f.Property))
                         ?.FilterValue;

            if (value == null)
                return default;

            return (T)Convert.ChangeType(value, typeof(T));
        }

        public static string? GetFilterValue(this LoadDataArgs e, params string[] properties)
        {
            return e?.Filters?
                    .FirstOrDefault(f => properties.Contains(f.Property))
                    ?.FilterValue?
                    .ToString();
        }


    }
}
