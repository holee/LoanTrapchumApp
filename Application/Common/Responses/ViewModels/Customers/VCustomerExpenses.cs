namespace Application.Responses.ViewModels.Customers
{
    public sealed class VCustomerExpenses
    {
        public long ExpendId { get; set; }
        public string? CustomerRefId { get; set; }
        public string? ExpenseTitle { get; set; }
        public decimal? ExpendAmount { get; set; }
        public string? ExDescription { get; set; }
    }
}
