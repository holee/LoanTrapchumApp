using TrapchumApp.Helpers.Enums;

namespace TrapchumApp.Helpers.Constants
{
    public static class FilterConstant
    {
        public static IReadOnlyList<FilterCustomer> CustomerFilters = new List<FilterCustomer>
        {
            new FilterCustomer(0, "All Customers"),
            new FilterCustomer(1, "My Customers"), 
            new FilterCustomer(2, "By Branch"),
        };
    }
}