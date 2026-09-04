namespace Application.Features.Expenses.Dto;

public sealed class ExpenseDtoCreate
{
    public ExpenseDtoCreate()
    {
    }

    public ExpenseDtoCreate(long id, string? customerRefId, string? expenseTitle, decimal? amount, string? description)
    {
        Id = id;
        CustomerRefId = customerRefId;
        ExpenseTitle = expenseTitle;
        Amount = amount;
        Description = description; 
    }

    public ExpenseDtoCreate(string? customerRefId, string? expenseTitle, decimal? amount, string? description)
    {
        CustomerRefId = customerRefId;
        ExpenseTitle = expenseTitle;
        Amount = amount;
        Description = description;
    }
    public long Id { get; set; }
    public string? CustomerRefId { get; set; } 
    public string? ExpenseTitle { get; set; }
    public decimal? Amount { get; set; } 
    public string? Description { get; set; }
}
