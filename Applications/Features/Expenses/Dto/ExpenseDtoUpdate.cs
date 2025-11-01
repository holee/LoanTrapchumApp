namespace Applications.Features.Expenses.Dto
{
    public sealed class ExpenseDtoUpdate   
    {
        public long Id { get; set; }
        public string? CustomerId { get; set; }
        public string? ExpenseTitle { get; set; }
        public decimal? Amount { get; set; }
        public string? Description { get; set; }

    }
}
