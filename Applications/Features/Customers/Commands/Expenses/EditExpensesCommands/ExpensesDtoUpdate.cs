namespace Applications.Features.Customers.Commands.ExpenseCommands.EditExpensCommands
{
    public sealed class ExpensesDtoUpdate   
    {
        public long Id { get; set; }
        public string? CustomerId { get; set; }
        public string? ExpenseTitle { get; set; }
        public decimal? Amount { get; set; }
        public string? Description { get; set; }

    }
}
