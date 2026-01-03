using TrapchumApp.Helpers.Options;

namespace TrapchumApp.Helpers.Constants;

public static class Filters
{
    public static IReadOnlyList<ListItem> CustomerFilters = new List<ListItem>
    {
        new ListItem(0, "All Customers"),
        new ListItem(1, "My Customers"),
        new ListItem(2, "By Branch"),
    };

    public static IReadOnlyList<ListItem> ScheduleFilters = new List<ListItem>
    {
        new ListItem(0, "All"),
        new ListItem(1, "Today"),
        new ListItem(2, "Month To Date"),
        new ListItem(3, "Over Due Date"),
        new ListItem(4, "Next Week"),
    };

}