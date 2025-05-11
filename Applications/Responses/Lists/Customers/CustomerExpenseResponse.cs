namespace Applications.Responses.Lists.Customers
{
    public class CustomerExpenseResponse  
    {
        public long Id { get; set; }
        public string? ExpenseTitle { get; set; }
        public decimal? Amount { get; set; } 
        public string? Description { get; set; }

    }
}
