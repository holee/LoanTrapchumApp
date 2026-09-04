using Radzen;

namespace Web.UI.Helpers.Extensions
{
    public static class LoadDataArgsExtensions
    {

        public static (string, bool) PageInfo(this LoadDataArgs args, string? sortColumn = null)
        {
            sortColumn ??= "Id";

            var sortDescriptor = args.Sorts?.FirstOrDefault();

            if (sortDescriptor is not null)
            {

                var sortField = sortDescriptor?.Property ?? sortColumn;

                var sortDescending = sortDescriptor?.SortOrder == SortOrder.Descending;

                return (sortField, sortDescending);

            }

            return (sortColumn, false);
        }

        public static (string SortField,
            string FilterProperty,
            bool IsDescending,
            int PageIndex,
            int PageSize) GetInfo(this LoadDataArgs args, string? sortColumn = null)
        {

            sortColumn ??= string.Empty;

            int pageIndex = args.Skip ?? 0;

            int pageSize = args.Top ?? 15;

            var sortDescriptor = args.Sorts?.FirstOrDefault();

            if (sortDescriptor is not null)
            {

                var sortField = sortDescriptor?.Property ?? sortColumn;

                var filterProperty = sortDescriptor?.Property ?? string.Empty;

                var sortDescending = sortDescriptor?.SortOrder == SortOrder.Descending;

                return (sortField, filterProperty, sortDescending, pageIndex, pageSize);

            }

            return (sortColumn, string.Empty, false, pageIndex, pageSize);
        }

        public static (string FilterProperty, T? FilterValue) FilterBy<T>(this LoadDataArgs args)
        {

            if (args.Filters is null || !args.Filters.Any()) return (string.Empty, default(T));

            var filterDescriptor = args.Filters.FirstOrDefault();

            var filterProperty = filterDescriptor?.Property ?? string.Empty;

            if (filterDescriptor?.FilterValue is null)
            {
                return (filterProperty, default(T));
            }

            var filterValue = (T)Convert.ChangeType(filterDescriptor.FilterValue, typeof(T));

            return (filterProperty, filterValue);

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

        public static string? FilterValue(this LoadDataArgs e, params string[] properties)
        {

            return e.Filters?
                     .FirstOrDefault(filter =>
                         properties.Contains(filter.Property) &&
                         filter.FilterValue is not null)
                     ?.FilterValue?
                     .ToString()?
                     .Trim() ?? string.Empty;

            //return e?.Filters?
            //        .FirstOrDefault(f => properties.Contains(f.Property))
            //        ?.FilterValue?
            //        .ToString();
        }

        public static string? Value(this LoadDataArgs e) => e?.Filters?.FirstOrDefault()?.FilterValue?.ToString() ?? string.Empty;

        public static (int Skip, int Take, string SortField, bool SortDescending) Page(this LoadDataArgs args, string? sortColumns = null)
        {
            int skip = args.Skip ?? 0;

            int take = args.Top ?? 15;

            if (args.Sorts!.Any())
            {
                var sortField = args.Sorts!.FirstOrDefault()?.Property ?? "Id";

                var sortDescending = args.Sorts!.FirstOrDefault()?.SortOrder == SortOrder.Descending;

                return (skip, take, sortField, sortDescending);

            }

            return (skip, take, sortColumns ?? "", false);
        }

        public static (int Skip, int Take, string SortField, bool SortDescending, string FilterValue) Page(this LoadDataArgs args, string? sortColumns = null, params string[] properties)
        {
            int skip = args.Skip ?? 0;

            int take = args.Top ?? 15;

            var filterValue = args.FilterValue(properties) ?? string.Empty;

            if (args.Sorts!.Any())
            {
                var sortField = args.Sorts!.FirstOrDefault()?.Property ?? "Id";

                var sortDescending = args.Sorts!.FirstOrDefault()?.SortOrder == SortOrder.Descending;

                return (skip, take, sortField, sortDescending, filterValue);

            }

            return (skip, take, sortColumns ?? "", false, filterValue);
        }


    }
}
