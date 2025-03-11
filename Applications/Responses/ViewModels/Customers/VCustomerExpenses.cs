namespace Applications.Responses.ViewModels.Customers
{
    public class VCustomerExpenses
    {
        public long ExpendId { get; set; }
        public long? CustomerRefId { get; set; }
        public string? ExpenseTitle { get; set; }
        public decimal? ExpendAmount { get; set; }
        public string? ExDescription { get; set; }
    }
}
