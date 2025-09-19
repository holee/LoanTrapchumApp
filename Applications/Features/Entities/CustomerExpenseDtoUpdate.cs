namespace Applications.Features.Entities
{
    public class CustomerExpenseDtoUpdate 
    {
        public long Id { get; set; }
        public int CustomerId { get; set; }
        public string? ExpenseTitle { get; set; }
        public decimal? Expense { get; set; }
        public string? Description { get; set; }

    }
}
