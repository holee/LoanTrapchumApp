namespace TrapchumApp.Helpers.Enums
{
    public enum CustomerFilter
    {
        All = 0,
        MyCustomer = 1,
        ByBranch = 2, 
    }


    public record struct FilterCustomer(int Id, string Name); 
}
