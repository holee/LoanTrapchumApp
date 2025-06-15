namespace TrapchumApp.Helpers.Extensions
{
    public static class LoadDataArgsExtensions
    {

        public static (string, bool) PageInfo(this LoadDataArgs args, string? sortColumns = null)
        {
            if (args.Sorts.Any())
            {
                var sortField = args.Sorts.FirstOrDefault()?.Property ?? "Id";

                var sortDescending = args.Sorts.FirstOrDefault()?.SortOrder == SortOrder.Ascending;

                return (sortField, sortDescending);

            }

            return (sortColumns ?? "", false);
        }

    }
}
